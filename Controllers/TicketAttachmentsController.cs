﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create()
        {
            ViewBag.TicketID = new SelectList(db.Tickets, "Id", "SubmitterId");
            ViewBag.UserID = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketID,FileName")] TicketAttachment ticketAttachment, HttpPostedFileBase newAttachment)
        {
            if (ModelState.IsValid)
            {

                if (newAttachment != null)
                {
                var justFileName = Path.GetFileNameWithoutExtension(newAttachment.FileName);
                justFileName = StringUtilities.URLFriendly(justFileName);
                justFileName = $"{justFileName}-{DateTime.Now.Ticks}";
                justFileName = $"{justFileName}{Path.GetExtension(newAttachment.FileName)}";

                newAttachment.SaveAs(Path.Combine(Server.MapPath("~/Attachments/"), justFileName));
                ticketAttachment.FilePath = $"/Attachments/{justFileName}";
                }


                ticketAttachment.Created = DateTime.Now;
                ticketAttachment.UserID = User.Identity.GetUserId();

                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();
                return RedirectToAction("Dashboard","Tickets",new {id= ticketAttachment.TicketID });
            }

            ViewBag.TicketID = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserID);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketID = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserID);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,TicketID,FilePath,Description,Created,FileUrl,FileName")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Tickets",new {id= ticketAttachment.TicketID });
            }
            ViewBag.TicketID = new SelectList(db.Tickets, "Id", "SubmitterId", ticketAttachment.TicketID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserID);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            var ticketId = ticketAttachment.TicketID;
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Tickets",new {id = ticketId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
