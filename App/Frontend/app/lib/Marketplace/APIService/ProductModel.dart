import 'package:flutter/material.dart';

class PanelModel
{
  bool isFavorite;
  final int PanelId;
  final String? Panelimage;
  final String? PanelBrandName;
  final String ? PanelPrice;
  final String? PanelCapacity;
  final String ? totalPrice;

  PanelModel({
    required this.isFavorite,
    required this.PanelId,
    required this.Panelimage,
    required this.PanelBrandName,
    required this.PanelCapacity,
    required this.PanelPrice,
    required this.totalPrice,
  });
  factory PanelModel.fromJson(json)
  {
    return PanelModel(
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



class BatteryModel
{
   bool isFavorite;
  final int BatteryID;
  final String Batteryimage;
  final String BatteryBrandName;
  final String BatteryPrice;
  final String BatteryCapacity;

  BatteryModel({required this.isFavorite,required this.BatteryID, required this.Batteryimage,required this.BatteryBrandName,required this.BatteryCapacity,required this.BatteryPrice});
  factory BatteryModel.fromJson(json)
  {
    return BatteryModel(
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

class InverterModel
{
   bool isFavorite;
  final int InverterID;

  final String Inverterimage;
  final String InverterBrandName;
  final String InverterPrice;
  final String InverterCapacity;

  InverterModel({required this.isFavorite, required this.InverterID,required this.Inverterimage,required this.InverterBrandName,required this.InverterCapacity,required this.InverterPrice, });
  factory InverterModel.fromJson(json)
  {
    return InverterModel(
      isFavorite: json['isFavorite'],
      InverterID: json['id'],
      Inverterimage: json['imageUrl'], 
      InverterBrandName: json['brand'],  
      InverterCapacity: json['capacity_Unit'],
      InverterPrice:json['priceStr'], 
    );
  }
}