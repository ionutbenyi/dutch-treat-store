using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.WebUtilities;
using DutchTreat.ViewModels;
using AutoMapper;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(bool includeItems=true)
        {
            try
            {
                var results = _repository.GetAllOrders(includeItems);
                return Ok(_mapper.Map< IEnumerable < Order > ,IEnumerable <OrderViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = _repository.GetOrderById(id);

                if (order != null) return Ok(_mapper.Map<Order, OrderViewModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order: {ex}");
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model)
        {
            // add it to db
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = _mapper.Map<OrderViewModel,Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }
                    _repository.AddEntity(newOrder);
                    if (_repository.SaveAll())
                    {
                       
                        return Created($"/api/orders/{newOrder.Id}", _mapper.Map<Order,OrderViewModel>(newOrder)); //Code 201 instead of 200
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed add order: {ex}");
                return BadRequest("Failed add order");
            }
            return BadRequest("Failed to save a new order");
        }
    }
}
