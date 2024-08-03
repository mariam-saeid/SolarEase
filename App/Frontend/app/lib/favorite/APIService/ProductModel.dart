import 'package:flutter/material.dart';

class FavoritePanel
{
  bool isFavorite;
  final int PanelId;
  final String? Panelimage;
  final String? PanelBrandName;
  final String ? PanelPrice;
  final String? PanelCapacity;
  final String ? totalPrice;

  FavoritePanel({
    required this.isFavorite,
    required this.PanelId,
    required this.Panelimage,
    required this.PanelBrandName,
    required this.PanelCapacity,
    required this.PanelPrice,
    required this.totalPrice,
  });
  factory FavoritePanel.fromJson(json)
  {
    return FavoritePanel(
      isFavorite: json['isFavorite'],
      PanelId: json['id'],
      Panelimage: json['imageUrl'], 
      PanelBrandName: json['brand'],
       PanelCapacity: json['capacity_Unit'], 
       PanelPrice: json['priceStr'], 
       totalPrice: json['totalPriceStr'], 

    );
  }


}
// -------------------------------------------------------------------



class FavoriteBattery
{
   bool isFavorite;
  final int BatteryID;
  final String Batteryimage;
  final String BatteryBrandName;
  final String BatteryPrice;
  final String BatteryCapacity;

  FavoriteBattery({required this.isFavorite,required this.BatteryID, required this.Batteryimage,required this.BatteryBrandName,required this.BatteryCapacity,required this.BatteryPrice});
  factory FavoriteBattery.fromJson(json)
  {
    return FavoriteBattery(
      isFavorite: json['isFavorite'],
      BatteryID: json['id'],
      Batteryimage:json['imageUrl'], 
      BatteryBrandName: json['brand'], 
      BatteryCapacity: json['capacity_Unit'], 
      BatteryPrice: json['priceStr'],
      );
  }

}

//--------------------------------------------------------------------

class FavoriteInverter
{
   bool isFavorite;
  final int InverterID;

  final String Inverterimage;
  final String InverterBrandName;
  final String InverterPrice;
  final String InverterCapacity;

  FavoriteInverter({required this.isFavorite, required this.InverterID,required this.Inverterimage,required this.InverterBrandName,required this.InverterCapacity,required this.InverterPrice, });
  factory FavoriteInverter.fromJson(json)
  {
    return FavoriteInverter(
      isFavorite: json['isFavorite'],
      InverterID: json['id'],
      Inverterimage: json['imageUrl'], 
      InverterBrandName: json['brand'],  
      InverterCapacity: json['capacity_Unit'],
      InverterPrice:json['priceStr'], 
    );
  }
}