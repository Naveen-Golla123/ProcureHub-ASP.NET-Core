using Microsoft.OpenApi.Any;
using Neo4j.Driver;
using Newtonsoft.Json.Linq;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.Collections;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace ProcureHub_ASP.NET_Core.Respositories
{
    public class LotsRepository : ILotsRepository
    {
        public readonly IConnectionDriver _driver;
        public LotsRepository(IConnectionDriver driver) 
        {
            _driver = driver;
        }

        public async Task<Lot> CreateItems(Lot lot)
        {
            IDriver connection = _driver.GetConnection();
            var session = connection.AsyncSession();

            try
            {

                string query = "MATCH (l:Lot) WHERE ID(l)=$lotId SET l.name = $lotName, l.description = $lotDescription WITH l as lot Unwind $lotItems as item CREATE (lot)-[r:HAS_ITEM]->(i:Item{name:item.Name, description: item.Description, basePrice: item.basePrice, quantity: item.quantity}) return lot";
                return await session.ExecuteWriteAsync(async t =>
                {
                    var reader = await t.RunAsync(query, new
                    {
                        lotId = lot._id,
                        lotName = lot.name,
                        lotDescription = lot.description,
                        lotItems = lot.has_item
                    });
                    Lot lot_ = new Lot();
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current["lot"].As<INode>();

                        lot_._id = (int)node.Id;
                        lot_.name = node.Properties["name"].As<string>();
                        lot_.description = node.Properties["description"].As<string>();
                        //lot.Items = node.Properties["Description"].As<List<Item>();
                    }
                    return lot_;
                }
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Lot> CreateLot(Lot lot)
        {
            IDriver connection = _driver.GetConnection();
            var session = connection.AsyncSession();

            try
            {

                string query = "MATCH (e:Event) WHERE ID(e) = $eventId CREATE (e)-[r:HAS_LOT]->(l:Lot{name:$lotName, description: $lotDescription}) WITH l AS lot UNWIND $lotItems as Item CREATE (lot)-[r1:HAS_ITEM]->(item:Item{name:Item.name,basePrice: Item.basePrice, quantity: Item.quantity}) RETURN lot";
                return await session.ExecuteWriteAsync(async t=> 
                {
                    var reader = await t.RunAsync(query, new
                    {
                        eventId = lot.EventId,
                        lotName = lot.name,
                        lotDescription = lot.description,
                        lotItems = lot.has_item
                    });
                    Lot lot_ = new Lot();
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current["lot"].As<INode>();
                        
                        lot_._id = (int)node.Id;
                        lot_.name = node.Properties["name"].As<string>();
                        lot_.description = node.Properties["description"].As<string>();
                        //lot.Items = node.Properties["Description"].As<List<Item>();
                    }

                    return lot_;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> DeleteLotById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Lot>> GetAllLots(int eventId_)
        {
            IDriver connection = _driver.GetConnection();
            var session = connection.AsyncSession();

            try
            {
                List<Lot> lots = new List<Lot>();
                string query = "MATCH path = (e)-[*]->(lot) where ID(e) = $eventId WITH collect(path) as paths CALL apoc.convert.toTree(paths) yield value RETURN value.has_lot as result;";
                var reader = await session.RunAsync(query, new
                {
                    eventId = eventId_
                });

                var list = await reader.ToListAsync();
                
                //while(await reader.FetchAsync())
                //{
                //    IDictionary<string, Object> node = reader.Current["value"].As<IDictionary<string,Object>>();
                //    Console.WriteLine(node);
                //    if(node != null)
                //    {
                //        var lots_ = node["has_lot"];

                //        foreach(var lot in (IEnumerable)lots_)
                //        {
                //            Lot lot_ = new Lot();
                //            //lot_.Name = lot.Name;
                //            //lot_.Description = lot.Description;
                //            lots.Append(lot_);
                //        }
                //    }
                //}

                foreach(var lot in list)
                {

                    var temp = JsonSerializer.Serialize(lot.Values["result"]);
                    lots = JsonSerializer.Deserialize<List<Lot>>(temp);

                }

                return lots;
            } 
            catch(Exception e)
            {
                throw e;
            }
        }

        public Task<Lot> GetLotById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Lot> UpdateItems(Lot lot)
        {
            IDriver connection = _driver.GetConnection();
            var session = connection.AsyncSession();

            try
            {

                string query = "MATCH (l:Lot) WHERE ID(l)=$lotId SET l.name = $lotName, l.description = $lotDescription WITH l as lot Unwind $lotItems as item MATCH (i:Item) WHERE ID(i) = item._id SET i.name = item.name, i.description = item.description, i.basePrice = item.basePrice, i.quantity = item.quantity return lot";
                return await session.ExecuteWriteAsync(async t => {
                    var reader = await t.RunAsync(query, new
                    {
                        lotId = lot._id,
                        lotName = lot.name,
                        lotDescription = lot.description,
                        lotItems = lot.has_item

                    });
                    Lot lot_ = new Lot();
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current["lot"].As<INode>();

                        lot_._id = (int)node.Id;
                        lot_.name = node.Properties["name"].As<string>();
                        lot_.description = node.Properties["description"].As<string>();
                        //lot.Items = node.Properties["Description"].As<List<Item>();
                    }

                    return lot_;
                }
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<Lot> UpdateLot(Lot lot)
        {
            throw new NotImplementedException();
        }

        public async Task<List<int>> DeleteLot(List<int> lotIds)
        {
            IDriver connection = _driver.GetConnection();
            var session = connection.AsyncSession();

            try
            {
                string query = "UNWIND $lotIds AS lotId Match (lot:Lot) where ID(lot)=lotId Detach delete lot return collect(lotId) as lotids";
                return await session.ExecuteWriteAsync(async t => {
                    var reader = await t.RunAsync(query, new
                    {
                        lotIds = lotIds
                    });
                    List <int> result = new List<int>();
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current["lotids"];
                        var temp = JsonSerializer.Serialize(node);
                        result = JsonSerializer.Deserialize<List<int>>(temp);
                        //lot_._id = (int)node.Id;
                    }
                    return result;
                }
                );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteItem(int id)
        {
            IDriver connection = _driver.GetConnection();
            var session = connection.AsyncSession();

            try
            {
                string query = "MATCH (n:Item)-[*0..]->(x) where ID(n) = $id DETACH DELETE x return n";
                return await session.ExecuteWriteAsync(async t => {
                    var reader = await t.RunAsync(query, new
                    {
                        id = id
                    });
                    Lot lot_ = new Lot();
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current["n"].As<INode>();
                        lot_._id = (int)node.Id;
                    }
                    return lot_._id;
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}





