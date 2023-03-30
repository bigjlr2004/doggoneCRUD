using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class WalkController : Controller
    {
        private readonly IWalkRepository _walkRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        public WalkController(
            IWalkRepository walkRepository, IDogRepository dogRepository, IWalkerRepository walkerRepository)
        {
            _walkRepo = walkRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
        }
        // GET: WalkController
        public ActionResult Index()
        {
            List<Walk> walks = _walkRepo.GetAllWalks();
            return View(walks);
        }

        // GET: WalkController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalkController/Create
        public ActionResult Create()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            List<Dog> dogs = _dogRepo.GetAllDogs();
            List <int> selectedDogs = new List<int>();
            WalkViewModel wvm = new WalkViewModel()
            {
                Walk = new Walk(),
                Dogs = dogs,
                Walkers = walkers,
                SelectedDogs = selectedDogs
            };

            return View(wvm);
            
        }

        // POST: WalkController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalkViewModel wvm)
        {
            try
            {
               
               foreach(int dogid in wvm.SelectedDogs) 
                {
                    Walk walk = new Walk()
                    {
                        Duration = wvm.Walk.Duration,
                        Date = wvm.Walk.Date,
                        WalkerId = wvm.Walk.WalkerId,
                        DogId = dogid

                    };
                    _walkRepo.AddAWalk(walk);
                   
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(wvm);
            }
           
        }

        // GET: WalkController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkController/Edit/5
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

        // GET: WalkController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkController/Delete/5
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
    }
}
