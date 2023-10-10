using BooxApp.Core.Extensions;
using BooxApp.Core.Common;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using BooxApp.Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DevExtreme.AspNet.Data;
using BooxApp.Module.ShipmentList.DTO;
using BooxApp.Entity;
using Dapper;
using System.Data;
using Newtonsoft.Json;
using BooxApp.Entity.DevExtremeModels;

namespace BooxApp.ShipmentList.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class order_ocean_v1Controller : ControllerBase  
    {  
        private readonly ILogger<order_ocean_v1Controller> _logger;
        private readonly DapperContext _ctx;
        public order_ocean_v1Controller(DapperContext context, ILogger<order_ocean_v1Controller> logger)  
        {  
            _ctx = context;  
            _logger = logger;
        } 
        
        [HttpPost]
        [Route("Batch/Default")]
        public async Task<IActionResult> BatchDefault([FromBody] List<DataChange> changes)
        {
            foreach (var change in changes)
            {
                if (change.Type == "insert" || change.Type == "update")
                {
                    var data = new order_ocean_v1DTO();
                    JsonConvert.PopulateObject(change.Data.ToString(), data);

                    if (!TryValidateModel(data))
                        return BadRequest(ModelState.GetFullErrorMessage());
                }
            }


            foreach (var change in changes)
            {
                order_ocean_v1DTO model = new order_ocean_v1DTO();
                var query = "";
                var parameters = new DynamicParameters();

                int? Id = null;                

                switch (change.Type)
                {               
                    case "insert":         


                        JsonConvert.PopulateObject(change.Data.ToString(), model);

                        query = "INSERT INTO order_ocean_v1 (order_no,carrier_name,total_qty,shipping_date,shipper_name,consignee_name,workflow_status) VALUES (@order_no,@carrier_name,@total_qty,@shipping_date,@shipper_name,@consignee_name,@workflow_status) ";
                        
                        
             parameters.Add("order_no" , model.order_no, DbType.String);
             parameters.Add("carrier_name" , model.carrier_name, DbType.String);
             parameters.Add("total_qty" , model.total_qty, DbType.Int32);
             parameters.Add("shipping_date" , model.shipping_date, DbType.DateTime);
             parameters.Add("shipper_name" , model.shipper_name, DbType.String);
             parameters.Add("consignee_name" , model.consignee_name, DbType.String);
             parameters.Add("workflow_status" , model.workflow_status, DbType.String);

  
                        using (var connection = _ctx.CreateConnection())
                        {
                            await connection.ExecuteAsync(query, parameters);
                        }

                        break;
                    case "update":                      
                        Id = Convert.ToInt32(change.Key);

                        using (var connection = _ctx.CreateConnection())
                        {
                            model = await connection.QueryFirstOrDefaultAsync<order_ocean_v1DTO>("SELECT * FROM order_ocean_v1 where Id = @Id", new { Id });
                        }

                        JsonConvert.PopulateObject(change.Data.ToString(), model);
                        
                        query = "UPDATE order_ocean_v1 SET  order_no = @order_no, carrier_name = @carrier_name, total_qty = @total_qty, shipping_date = @shipping_date, shipper_name = @shipper_name, consignee_name = @consignee_name, workflow_status = @workflow_status WHERE Id = @Id ";
                        
                        
                      parameters.Add("Id" , Id, DbType.Int32);
             parameters.Add("order_no" , model.order_no, DbType.String);
             parameters.Add("carrier_name" , model.carrier_name, DbType.String);
             parameters.Add("total_qty" , model.total_qty, DbType.Int32);
             parameters.Add("shipping_date" , model.shipping_date, DbType.DateTime);
             parameters.Add("shipper_name" , model.shipper_name, DbType.String);
             parameters.Add("consignee_name" , model.consignee_name, DbType.String);
             parameters.Add("workflow_status" , model.workflow_status, DbType.String);

  
                        using (var connection = _ctx.CreateConnection())
                        {
                            await connection.ExecuteAsync(query, parameters);
                        }

                        break;
                    case "remove":
                        Id = Convert.ToInt32(change.Key);

                        query = "DELETE FROM order_ocean_v1 WHERE Id = @Id";
                        using (var connection = _ctx.CreateConnection())
                        {
                            await connection.ExecuteAsync(query, new { Id });
                        }
                        break;
                    default:
                        break;
                }

            }

            return Ok(new ApiResponse(200));
        }
  
        // GET: api/order_ocean_v1/GetAll  
        [HttpGet, Route("GetAll")]  
        public async Task<IActionResult> GetAll(DataSourceLoadOptions loadOptions)
        {             
            try
            {
                IEnumerable<dynamic> data = null; 

                using (var connection = _ctx.CreateConnection())
                {
                    data = await connection.QueryAsync<dynamic>("SELECT * FROM order_ocean_v1");                    
                }

                return Ok(DataSourceLoader.Load(data, loadOptions));
            }
            catch (Exception e)
            {
                _logger.LogError("Error at GetAll order_ocean_v1 with exception : {@ex}", e);

                var messages = new List<string>();
                do
                {
                    messages.Add(e.Message);
                    e = e.InnerException;
                }
                while (e != null);
                var message = string.Join(" - ", messages);

                return Ok(message);
            }
        }  
  
        // GET api/order_ocean_v1/GetByID/5  
        [HttpGet, Route("GetByID/{id}")]  
        public async Task<IActionResult> GetByID(int id)  
        {  
            try
            {
                dynamic data = null;

                using (var connection = _ctx.CreateConnection())
                {
                    data = await connection.QueryFirstOrDefaultAsync<dynamic>("SELECT * FROM order_ocean_v1 where Id = @Id", new { id });
                }

                return Ok(new ApiOkResponse(data));
            }
            catch (Exception e)
            {
                _logger.LogError("Error at GetByID order_ocean_v1 with exception : {@ex}", e);

                var messages = new List<string>();
                do
                {
                    messages.Add(e.Message);
                    e = e.InnerException;
                }
                while (e != null);
                var message = string.Join(" - ", messages);

                return Ok(message);
            }           
        }  
  
  
        // POST api/order_ocean_v1/Save   
        [HttpPost, Route("Save")]  
        public async Task<IActionResult> Save([FromBody]order_ocean_v1DTO model)  
        {  
            ApiResponse result = null; 

            if (model == null)  
            {  
                return BadRequest();  
            }  
           
            try  
            {  
                var query = "INSERT INTO order_ocean_v1 (order_no,carrier_name,total_qty,shipping_date,shipper_name,consignee_name,workflow_status) VALUES (@order_no,@carrier_name,@total_qty,@shipping_date,@shipper_name,@consignee_name,@workflow_status) ";
                        
                var parameters = new DynamicParameters();
             parameters.Add("order_no" , model.order_no, DbType.String);
             parameters.Add("carrier_name" , model.carrier_name, DbType.String);
             parameters.Add("total_qty" , model.total_qty, DbType.Int32);
             parameters.Add("shipping_date" , model.shipping_date, DbType.DateTime);
             parameters.Add("shipper_name" , model.shipper_name, DbType.String);
             parameters.Add("consignee_name" , model.consignee_name, DbType.String);
             parameters.Add("workflow_status" , model.workflow_status, DbType.String);

  
                using (var connection = _ctx.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }

                    
                result = new ApiResponse(200);
            }  
            catch (Exception e)  
            {  
                _logger.LogError("Error at Save order_ocean_v1 with exception : {@ex}", e);

                var messages = new List<string>();
                do
                {
                    messages.Add(e.Message);
                    e = e.InnerException;
                }
                while (e != null);
                var message = string.Join(" - ", messages);
                                  
                result = new ApiResponse(500, message);
                        
            }  

            return Ok(result);  
        }  

        
        // Put #UrlPutByID   
        [HttpPut, Route("Update")]  
        public async Task<IActionResult> Update([FromBody]order_ocean_v1DTO model, int Id)  
        {  
            ApiResponse result = null; 

            if (model == null)  
            {  
                return BadRequest();  
            }  
           
            try  
            {  
                var query = "UPDATE order_ocean_v1 SET  order_no = @order_no, carrier_name = @carrier_name, total_qty = @total_qty, shipping_date = @shipping_date, shipper_name = @shipper_name, consignee_name = @consignee_name, workflow_status = @workflow_status WHERE Id = @Id ";
                        
                var parameters = new DynamicParameters();
                      parameters.Add("Id" , Id, DbType.Int32);
             parameters.Add("order_no" , model.order_no, DbType.String);
             parameters.Add("carrier_name" , model.carrier_name, DbType.String);
             parameters.Add("total_qty" , model.total_qty, DbType.Int32);
             parameters.Add("shipping_date" , model.shipping_date, DbType.DateTime);
             parameters.Add("shipper_name" , model.shipper_name, DbType.String);
             parameters.Add("consignee_name" , model.consignee_name, DbType.String);
             parameters.Add("workflow_status" , model.workflow_status, DbType.String);

  
                using (var connection = _ctx.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }

                    
                result = new ApiResponse(200);
            }  
            catch (Exception e)  
            {  
                _logger.LogError("Error at Update order_ocean_v1 with exception : {@ex}", e);

                var messages = new List<string>();
                do
                {
                    messages.Add(e.Message);
                    e = e.InnerException;
                }
                while (e != null);
                var message = string.Join(" - ", messages);
                                  
                result = new ApiResponse(500, message);
                        
            }  

            return Ok(result);  
        }  
        
  
        // DELETE api/order_ocean_v1/DeleteByID/5  
        [HttpDelete, Route("DeleteByID/{id}")]  
        public async Task<IActionResult> DeleteByID(int id)  
        {  
            ApiResponse result = null;  

            try  
            {  
                var query = "DELETE FROM order_ocean_v1 WHERE Id = @Id";
                using (var connection = _ctx.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { id });
                }
                result = new ApiResponse(200);
            }  
            catch (Exception e)  
            {  
                _logger.LogError("Error at UpdateByID order_ocean_v1 with exception : {@ex}", e);

                var messages = new List<string>();
                do
                {
                    messages.Add(e.Message);
                    e = e.InnerException;
                }
                while (e != null);
                var message = string.Join(" - ", messages);
                                  
                result = new ApiResponse(500, message); 
            }   
            return Ok(result);  
        }  
    }  
}