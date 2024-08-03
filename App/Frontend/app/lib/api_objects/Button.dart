import 'package:app/addpost.dart';
import 'package:app/api_objects/url.dart';
import 'package:app/homeuser.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';


class Button {
  String message;
  int id;
  Button({required this.id, required this.message});
}

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

ApiService a = ApiService();
/****************** api to get initial chat and buttons************************************ */
Future<bool> initialchat(
    List<String> chatMessages, List<Button> mainMinue) async {
  chatMessages.clear();
  mainMinue.clear();
  int id = user.id!;
  print(id);
  try {
    /***********************Chat Messages**********************************/
    var response = await a._dio
        .get(hosturl+"api/ChatbotMessages/ByPerson/$id");
    for (var m in response.data) {
      chatMessages.add("You: " + m["question"].replaceAll(';', '\n'));
      chatMessages.add("Bot: " + m["answer"].replaceAll(';', '\n'));
    }

    /***********************Main Buttons**********************************/
    response =
        await a._dio.get(hosturl+"api/FAQCategories");
    for (var b in response.data) {
      Button temp = Button(id: b["id"], message: b["name"]);
      mainMinue.add(temp);
    }
    
    return true;
  } catch (e) {
    print(e);
    return false;
  }
}

/**************api for back********************************/
Future<bool> backClick(List<Button> mainMinue) async {
  mainMinue.clear();
  try {
    /***********************Main Buttons**********************************/
    var response =
        await a._dio.get(hosturl+"api/FAQCategories");
    for (var b in response.data) {
      Button temp = Button(id: b["id"], message: b["name"]);
      mainMinue.add(temp);
    }
    return true;
  } catch (e) {
    print(e);
    return false;
  }
}

/****************api for main*************************/
Future<bool> clikmain(List<Button> mainMenu, int buttonid) async {
  try {
    print(buttonid);

    var response = await a._dio
        .get(hosturl +"api/FAQs/categoryId/$buttonid");
    mainMenu.clear();
    for (var b in response.data) {
      Button temp = Button(id: b["id"], message: b["question"]);
      mainMenu.add(temp);
    }
    return true;
  } catch (e) {
    print(e);
    return false;
  }
}

/******************api for answer and new menue************************/
Future<bool> clickOnQuestion(
    List<String> chatMessages, List<Button> mainMenu, int buttonid) async {
  int id = user.id!;

  try {
    var response = await a._dio.post(
        hosturl+"api/ChatbotMessages/Create/$id/$buttonid");

    /**********************Chat *********************************/
    chatMessages.add("Bot: " + response.data["answer"].replaceAll(';', '\n'));
    return true;
  } catch (e) {
    print(e);
    return false;
  }
}
