using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {

        private readonly IWalkerRepository _walkerRepo;
        private readonly IOwnerRepository _ownerRepo;
        
        public WalkersController(IWalkerRepository walkerRepository, IOwnerRepository ownerRepository)
        {
            _walkerRepo = walkerRepository;
            _ownerRepo = ownerRepository;
           

        }

        // GET: Walkers
        public ActionResult Index()
        {
           
                        
                int ownerId = GetCurrentUserId();

            if (ownerId == 0)
            {
                List<Walker> allwalkers = _walkerRepo.GetAllWalkers();
                return View(allwalkers);
            } else
            {
                
                Owner owner = _ownerRepo.GetOwnerById(ownerId);
                List<Walker> neighborhoodWalkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);
                return View(neighborhoodWalkers);
            }
    
        }

        // GET: WalkersController/Details/5
        // GET: Walkers/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List < Walk > walks = _walkerRepo.WalksbyWalkerId(id);
            Int32 TotalWalkTime = _walkerRepo.TotalWalksTime(id);

            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                Walker = walker,
                Walks = walks,
                TotalWalkTime = TotalWalkTime
            };

            if (walker == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != null)
            {
            return int.Parse(id);
            }
            return 0;
        }
    }
}
