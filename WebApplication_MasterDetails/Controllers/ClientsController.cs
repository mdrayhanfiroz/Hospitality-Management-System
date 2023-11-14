using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WebApplication_MasterDetails.Models;
using WebApplication_MasterDetails.Models.ViewModels;

namespace WebApplication_MasterDetails.Controllers
{
    public class ClientsController : Controller
    {
        TravelDbContext db = new TravelDbContext();
        public ActionResult Index()
        {
            var clients = db.Clients.Include(x => x.BookingEntries.Select(b => b.Spot)).OrderByDescending(x => x.ClientId).ToList();
            return View(clients);
        }
        public ActionResult AddNewSpot(int? id)
        {
            ViewBag.spots = new SelectList(db.Spots.ToList(), "SpotId", "SpotName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewSpot");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ClientViewModel cvm, int[] spotId)
        {
            if(ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientName = cvm.ClientName,
                    DateOfBirth = cvm.DateOfBirth,
                    Age = cvm.Age,
                    MaritalStatus = cvm.MaritalStatus,
                };

                HttpPostedFileBase file = cvm.PictureFile;

                if(file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

                    file.SaveAs(Server.MapPath(filePath));
                    client.Picture = filePath;
                }

                if(spotId != null)
                {
                    foreach (var item in spotId)
                    {
                        BookingEntry entry = new BookingEntry()
                        {
                            Client = client,
                            SpotId = item
                        };
                        db.BookingEntries.Add(entry);
                    }
                }

                db.SaveChanges();
                return PartialView("_success");
            }

            return PartialView("_error");
        }

        public ActionResult Edit(int? id)
        {

            Client client = db.Clients.First(x => x.ClientId == id);
            var clientSpots = db.BookingEntries.Where(x => x.ClientId == id).ToList();
            ClientViewModel cvm = new ClientViewModel()
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName,
                Age = client.Age,
                DateOfBirth = client.DateOfBirth,
                Picture = client.Picture,
                MaritalStatus = (bool)client.MaritalStatus
            };
            if (clientSpots.Count() > 0)
            {
                foreach (var item in clientSpots)
                {
                    cvm.SpotList.Add(item.SpotId);
                }
            }
            return View(cvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientViewModel cvm, int[] spotId)
        {
            
            if (ModelState.IsValid)
            {
                // Retrieve the existing client from the database
                Client client = db.Clients.FirstOrDefault(x => x.ClientId == cvm.ClientId);

                if (client != null)
                {
                    // Update the client's information
                    client.ClientName = cvm.ClientName;
                    client.DateOfBirth = cvm.DateOfBirth;
                    client.Age = cvm.Age;
                    client.MaritalStatus = cvm.MaritalStatus;

                    HttpPostedFileBase file = cvm.PictureFile;
                    if (file != null)
                    {
                        // A new picture file is uploaded, update the picture property
                        string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(Server.MapPath(filePath));
                        client.Picture = filePath;
                    }

                    // Update the existing spot entries

                    // Delete existing spot entries for the client
                    var existingSpotEntries = db.BookingEntries.Where(x => x.ClientId == client.ClientId).ToList();
                    foreach (var entry in existingSpotEntries)
                    {
                        db.BookingEntries.Remove(entry);
                    }

                    // Add new spot entries
                    if (spotId != null)
                    {
                        foreach (var item in spotId)
                        {
                            BookingEntry bookingEntry = new BookingEntry()
                            {
                                ClientId = client.ClientId,
                                SpotId = item
                            };
                            db.BookingEntries.Add(bookingEntry);
                        }
                    }

                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                    return PartialView("_success");
                }
                else
                {
                    // Handle the case where the client with the given ID was not found.
                    return HttpNotFound();
                }
            }
            return PartialView("_error", cvm);
        }
        public ActionResult Delete(int id)
        {
            var client = db.Clients.Find(id);

            if(client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DoDelete(int id)
        {
            var client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            // Delete associated BookingEntry records first
            var bookingEntries = db.BookingEntries.Where(be => be.ClientId == id).ToList();
            foreach (var entry in bookingEntries)
            {
                db.BookingEntries.Remove(entry);
            }

            // Then delete the Client
            db.Clients.Remove(client);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}