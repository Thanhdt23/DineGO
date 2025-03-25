using System.Collections.Generic;
using DineGO_Api.Model;

public interface IRestaurantOwnerRepository
{
    List<RestaurantOwner> GetRestaurantOwners();
    RestaurantOwner FindRestaurantOwnerById(int ID);
    void SaveRestaurantOwner(RestaurantOwner owner);
    void UpdateRestaurantOwner(RestaurantOwner owner);
    void DeleteRestaurantOwner(int ownerId);
}