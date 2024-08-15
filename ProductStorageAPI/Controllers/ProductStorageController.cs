using SharedModels.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ProductStorageAPI.DTO.Mapping;
using ProductStorageAPI.DTO;
using System.Xml.Linq;
using ProductStorageAPI.Repository;

namespace ProductStorageAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductStorageController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly Mapper _mapper;
        private readonly ProductStorageRepository _repo;

        public ProductStorageController(ProductContext context, ProductStorageRepository repo)
        {
            _context = context;
            _repo = repo;
            _mapper = new Mapper(new MapperConfiguration(config => config.AddProfile(new MappingProfile())));
        }

        [HttpGet(template: "GetStorages")]
        public ActionResult GetStorages()
        {
            return Ok(_repo.GetStorages());
        }

        [HttpPost(template: "AddStorage")]
        public ActionResult AddStorage([FromQuery] string name, string description)
        {
            if (name.Length > 0)
            {
                int currentId = _repo.GetStorageId(name);
                if (currentId == -1)
                {
                    return Ok(_repo.AddStorage(name, description));
                }
                else return StatusCode(400, $"Склад с именем \"{name}\" уже существует (ID={currentId}).");
            }
            else return StatusCode(400, "Имя склада не может быть пустым.");
        }

        [HttpGet(template:"GetStorageId")]
        public ActionResult GetStorageId(string name)
        {
            return Ok(_repo.GetStorageId(name));
        }

        [HttpDelete(template: "DeleteStorage")]
        public ActionResult DeleteStorage([FromQuery] int storageId)
        {
            var result = _context.Storages.FirstOrDefault(x => x.ID == storageId);
            if (result != null)
            {
                _context.Storages.Remove(result);
                _context.SaveChanges();
                return Ok();
            }
            else return StatusCode(400, $"Склада с ID={storageId} не существует.");
            
        }

        [HttpGet("GetProductsAllocation")]
        public ActionResult GetProductsAllocation()
        {
            return Ok(_repo.GetProductsAllocation());
        }

        [HttpPost(template:"AddProductsToStorage")]
        public ActionResult AddProductsToStorage([FromQuery] int productId, int storageId, int count)
        {
            return HandleProducts(productId, storageId, count, (p, s, c) =>
            {
                var result = new ProductStorage()
                {
                    Name = $"Завоз {DateTime.Now}",
                    ProductID = productId,
                    StorageID = storageId,
                    Count = count
                };
                _context.ProductStorages.Add(result);
                _context.SaveChanges();
                return Ok(result.ID);
            });
        }

        [HttpPost(template:"PickUpProductsFromStorage")]
        public ActionResult PickUpProductsFromStorage([FromQuery] int productId, int storageId, int count)
        {
            return HandleProducts(productId, storageId, count, (p, s, c) =>
            {
                var result = new ProductStorage()
                {
                    Name = $"Отгрузка {DateTime.Now}",
                    ProductID = productId,
                    StorageID = storageId,
                    Count = -count
                };
                _context.ProductStorages.Add(result);
                _context.SaveChanges();
                return Ok(result.ID);
            });
        }

        [HttpPost(template:"MoveProducts")]
        public ActionResult MoveProducts([FromQuery] int productId, int storageFromId, int storageToId, int count)
        {
            if (VerifyProduct(productId))
            {
                if (VerifyStorage(storageFromId))
                {
                    if (VerifyStorage(storageToId))
                    {
                        if (count > 0)
                        {
                            string name = $"Перемещение {DateTime.Now}";
                            var removed = new ProductStorage()
                            {
                                Name = name,
                                ProductID = productId,
                                StorageID = storageFromId,
                                Count = -count
                            };
                            _context.ProductStorages.Add(removed);

                            var added = new ProductStorage()
                            {
                                Name = name,
                                ProductID = productId,
                                StorageID = storageToId,
                                Count = count
                            };
                            _context.ProductStorages.Add(added);

                            _context.SaveChanges();
                            return Ok("Перемещение успешно.");
                        }
                        return StatusCode(400, "Нельзя перемещать отрицательное или нулевое количество продуктов.");
                    }
                    else return StatusCode(400, $"Склад с ID={storageToId} не зарегистрирован.");
                }
                else return StatusCode(400, $"Склад с ID={storageFromId} не зарегистрирован.");
            }
            else return StatusCode(400, $"Продукт с ID={productId} не зарегистрирован.");
        }

        [HttpGet(template:"GetStorageTotalProduct")]
        public ActionResult GetStorageTotalProducts(int storageId)
        {
            if (VerifyStorage(storageId))
            {
                var dbRecords = _context.ProductStorages.Where(x => x.StorageID == storageId).ToList();
                var productIdList = dbRecords.Select(x => x.ProductID).Distinct().ToList();
                var result = new StorageСontentDto()
                {
                    ProductsCount = productIdList.ToDictionary(
                        x => x ?? 0,
                        x => dbRecords.Where(y => y.ProductID == x).Select(y => y.Count).Aggregate((sum, next) => sum + next) ?? 0)
                };
                return Ok(result);
            }
            else return StatusCode(400, $"Склад с ID={storageId} не зарегистрирован.");
        }

        private bool VerifyProduct(int productId)
        {
            return _context.Products.Any(x => x.ID == productId);
        }
        private bool VerifyStorage(int storageId)
        {
            return _context.Storages.Any(x => x.ID == storageId);
        }
        private ActionResult HandleProducts(int productId, int storageId, int count, Func<int, int, int, ActionResult> handling)
        {
            if (VerifyProduct(productId))
            {
                if (VerifyStorage(storageId))
                {
                    if (count > 0)
                    {
                        return handling.Invoke(productId, storageId, count);
                    }
                    return StatusCode(400, "Нельзя добавлять или отгружать отрицательное или нулевое количество продуктов.");
                }
                else return StatusCode(400, $"Склад с ID={storageId} не зарегистрирован.");
            }
            else return StatusCode(400, $"Продукт с ID={productId} не зарегистрирован.");
        }
    }
}
