import 'dart:core';
import 'dart:core';
import 'package:app/api_objects/inputCalculations.dart';
import 'package:app/api_objects/url.dart';
import 'package:app/api_objects/user.dart';
import 'package:app/homeuser.dart';
import 'package:flutter/material.dart';
import 'package:dio/dio.dart';

/////////////////////////////////////////////////
class ApiService {
  final Dio _dio = Dio();
  String _token = user.useKey!;

  ApiService() {
    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) {
          if (_token != null) {
            options.headers['Authorization'] = 'Bearer $_token';
          }
          return handler.next(options); // Continue with the request
        },
        onError: (DioError e, handler) {
          // Handle errors globally
          print(e.response?.data);
          return handler.next(e); // Continue with the error
        },
      ),
    );
  }
}

/////////////////////////////////////////////////////////
class System {
  int? systemid = 0;
  String? totalStstemSize = "";
  String? totalcost = "";
  String? payback = "";
  Device panel = Device();
  Device inverter = Device();
  Device battery = Device();
  Saving financial = Saving();
  Saving enviromental = Saving();
  Calculationsdata inpudata;
  String roofSpace = "0";
  System({required this.inpudata});

  Map<String, dynamic> tojson() {
    print(inpudata.maxload.runtimeType);
    if (inpudata.ongrid) {
      return {
        "MaxMonthLoad": inpudata.maxusageon,
        "Grid": inpudata.ongrid,
        "ElectricalCoverage": inpudata.elecitycoverage,
        "Devicesload": inpudata.maxload
      };
    } else {
      return {
        "MaxMonthLoad": inpudata.maxusageoff,
        "Grid": inpudata.ongrid,
        "ElectricalCoverage": inpudata.elecitycoverage,
        "Devicesload": inpudata.maxload
      };
    }
  }

//////////////////////////////////////////////////
  Map<String, dynamic> tojsonyear() {
    return {
      "January": this.inpudata.electricalBills.months["January"],
      "February": this.inpudata.electricalBills.months["February"],
      "March": this.inpudata.electricalBills.months["March"],
      "April": this.inpudata.electricalBills.months["April"],
      "May": this.inpudata.electricalBills.months["May"],
      "June": this.inpudata.electricalBills.months["June"],
      "July": this.inpudata.electricalBills.months["July"],
      "August": this.inpudata.electricalBills.months["August"],
      "September": this.inpudata.electricalBills.months["September"],
      "October": this.inpudata.electricalBills.months["October"],
      "November": this.inpudata.electricalBills.months["November"],
      "December": this.inpudata.electricalBills.months["December"],
      "Grid": inpudata.ongrid,
      "ElectricalCoverage": inpudata.elecitycoverage,
      "Devicesload": inpudata.maxload
    };
  }

  /****************api for calculate offgrid and ongrid with maxusage*******************/
  Future<bool> getSystemInfomax() async {
    ApiService a = ApiService();
    try {
      int? id = user.id;

      var response = await a._dio.post(
          hosturl+"api/Calculators/CreateByMaxLoad/$id",
          data: FormData.fromMap(tojson()));
      print("*********************");
      print(tojson());
      print(response);

      this.totalStstemSize = response.data["systemSize"];
      this.totalcost = response.data["totalCost"];
      this.roofSpace = response.data["roofSpace"];

      this.panel.capacity = response.data["panelsCapacity"];
      this.panel.numm = response.data["numofPanels"];
      this.panel.cost = response.data["panelsPrice"];

      this.inverter.capacity = response.data["inverterCapacity"];
      this.inverter.numm = "1";
      this.inverter.cost = response.data["inverterPrice"];

      this.enviromental.monthly = response.data["environmentalBenefitMonthly"];
      this.enviromental.yearly = response.data["environmentalBenefitYearly"];
      this.enviromental.year25 =
          response.data["environmentalBenefitTwentyFiveYear"];

      // if (!inpudata.ongrid) {
      this.battery.capacity = response.data["batteryCapacity"] ?? "0";
      this.battery.numm = response.data["numofBatteries"] ?? "0";
      this.battery.cost = response.data["batteryPrice"] ?? "0";
      // }

      if (inpudata.ongrid) {
        this.financial.monthly = response.data["financialSavingMonthly"];
        this.financial.yearly = response.data["financialSavingYearly"];
        this.financial.year25 = response.data["financialSavingTwentyFiveYear"];
        this.payback = response.data["paybackPeriod"];
      }

      return true;
    } catch (e) {
      print(e);
      return false;
    }
  }

  /***************api for calculate system details using Electricity bills******************/
  Future<bool> getSystemInfoyear() async {
    ApiService a = ApiService();
    try {
      int? id = user.id;
      var response = await a._dio.post(
          hosturl+"api/Calculators/CreateByElectricityBills/$id",
          data: FormData.fromMap(tojsonyear()));
      print(response);
      this.totalStstemSize = response.data["systemSize"];
      this.totalcost = response.data["totalCost"];
      this.payback = response.data["paybackPeriod"];
      this.roofSpace = response.data["roofSpace"];

      this.panel.capacity = response.data["panelsCapacity"];
      this.panel.numm = response.data["numofPanels"];
      this.panel.cost = response.data["panelsPrice"];

      this.inverter.capacity = response.data["inverterCapacity"];
      this.inverter.numm = "1";
      this.inverter.cost = response.data["inverterPrice"];

      this.battery.capacity = response.data["batteryCapacity"] ?? "0";
      this.battery.numm = response.data["numofBatteries"] ?? "0";
      this.battery.cost = response.data["batteryPrice"] ?? "0";

      this.financial.monthly = response.data["financialSavingMonthly"];
      this.financial.yearly = response.data["financialSavingYearly"];
      this.financial.year25 = response.data["financialSavingTwentyFiveYear"];

      this.enviromental.monthly = response.data["environmentalBenefitMonthly"];
      this.enviromental.yearly = response.data["environmentalBenefitYearly"];
      this.enviromental.year25 =
          response.data["environmentalBenefitTwentyFiveYear"];

      return true;
    } catch (e) {
      print(e);
      return false;
    }
  }

  /*************************Check if user calculate before************************/
  Future<int> isExist() async {
    ApiService a = ApiService();
    try {
      int id = user.id!;
      print(id);
      var response = await a._dio.get(
        hosturl+"api/Calculators/personId/$id",
      );

      // user.systemsize = response.data["systemSize"];
      this.totalStstemSize = response.data["systemSize"];
      this.roofSpace = response.data["roofSpace"];
      this.totalcost = response.data["totalCost"];

      this.panel.capacity = response.data["panelsCapacity"];
      this.panel.numm = response.data["numofPanels"];
      this.panel.cost = response.data["panelsPrice"];

      this.inverter.capacity = response.data["inverterCapacity"];
      this.inverter.numm = "1";
      this.inverter.cost = response.data["inverterPrice"];

      this.battery.capacity = response.data["batteryCapacity"] ?? "0";
      this.battery.numm = response.data["numofBatteries"] ?? "0";
      this.battery.cost = response.data["batteryPrice"] ?? "0";

      this.financial.monthly = response.data["financialSavingMonthly"] ?? "0";
      this.financial.yearly = response.data["financialSavingYearly"] ?? "0";
      this.financial.year25 =
          response.data["financialSavingTwentyFiveYear"] ?? "0";
      this.payback = response.data["paybackPeriod"] ?? "0";

      this.enviromental.monthly = response.data["environmentalBenefitMonthly"];

      this.enviromental.yearly = response.data["environmentalBenefitYearly"];

      this.enviromental.year25 =
          response.data["environmentalBenefitTwentyFiveYear"];

      print("done");
      return response.statusCode!;
    } catch (e) {
      print(e);
      return 404;
    }
  }

  /*********************api for remove system details************************/
  Future<bool> remove() async {
    ApiService a = ApiService();
    try {
      int id = user.id!;

      var response = await a._dio.delete(
        hosturl+"api/Calculators/DeleteAllByPersonId/$id",
      );

      return true;
    } catch (e) {
      print(e);
      return false;
    }
  }
}

class Device {
  String numm = "0";
  String capacity = "0";
  String cost = "0";
  String name = "0";
}

class Saving {
  String monthly = "0";
  String yearly = "0";
  String year25 = "0";
}
