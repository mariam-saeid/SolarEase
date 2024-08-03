import 'package:app/MarketPlace/APIService/ProductModel.dart';
import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';

class ApiService {
  final Dio _dio = Dio();

  ApiService() {
    String _token = user.useKey!;
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

ApiService a = ApiService();

class PanelService {
  final Dio dio;
  PanelService(this.dio);
  int ID = user.id!;

  String query = 'Panel';
  Future<List<PanelModel>> getPanelInfo() async {
    try {
      Response response = await a._dio.get(
          hosturl+'api/SolarProducts/$ID?query=$query');
      List<dynamic> jsonData = response.data;

      List<PanelModel> PanelList = [];

      for (var item in jsonData) {
        PanelModel panelModel = PanelModel.fromJson(item);
        PanelList.add(panelModel);
      }

      return PanelList;
    } on DioException catch (e) {
      throw Exception("Oops, there was an error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }

  Future<void> AddToFavorite(int panelId) async {
    print(panelId);
    try {
      final response = await a._dio.put(
        hosturl+'api/FavoriteProducts/ToggleFavorite/$ID/$panelId', // Replace with your API endpoint
      );
      print(response.statusCode);
      if (response.statusCode == 200) {
        // Handle successful response
        print('Favorite status updated successfully');
      } else {
        // Handle other status codes
        print('Failed to update favorite status');
      }
    } catch (e) {
      // Handle error
      print('Error updating favorite status: $e');
    }
  }
}
// ------  --------------- ------------------------ --------------------------

class BatteryService {
  final Dio dio;
  int ID = user.id!;

  BatteryService(this.dio);
  String query = 'Battery';
  Future<List<BatteryModel>> getBatteryInfo() async {
    try {
      Response response = await a._dio.get(
          hosturl+'api/SolarProducts/$ID?query=$query');
      List<dynamic> jsonData = response.data;

      List<BatteryModel> BatteryList = [];

      for (var item in jsonData) {
        BatteryModel batteryModel = BatteryModel.fromJson(item);
        BatteryList.add(batteryModel);
      }

      return BatteryList;
    } on DioException catch (e) {
      throw Exception("Oops, there was an error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }
}

//- -----------------------------------------------------------------------------------

class InverterService {
  final Dio dio;
  int ID = user.id!;

  InverterService(this.dio);
  String query = 'Inverter';
  Future<List<InverterModel>> getInverterInfo() async {
    try {
      Response response = await a._dio.get(
          hosturl+'api/SolarProducts/$ID?query=$query');

      // Assuming response.data is a List of maps
      List<dynamic> jsonData = response.data;

      List<InverterModel> PanelList = [];

      for (var item in jsonData) {
        InverterModel inverterModel = InverterModel.fromJson(item);
        PanelList.add(inverterModel);
        // if (item["categoryName"] == "Inverter") {
        //   InverterModel inverterModel = InverterModel.fromJson(item);
        //   PanelList.add(inverterModel);
        // }
      }

      return PanelList;
    } on DioException catch (e) {
      throw Exception("Oops, there was an error, try later");
    } catch (e) {
      throw Exception("Oops, there was an error, try later");
    }
  }

  //---''
  // favorite_service.dart
}

