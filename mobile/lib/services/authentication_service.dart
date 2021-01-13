import 'package:http/http.dart' as http;
import 'dart:convert';

class AuthenticationService{

  AuthenticationService();

  Future<bool> isPhoneNumberRegistered(String url) async{
    http.Response response = await http.get(url);
    if(response.statusCode == 200){
      bool data = response.body.toLowerCase() == 'true';
      return data;
    }
    else{
      //TODO return correct answer
      print(response.statusCode);
      return null;
    }
  }

  Future<bool> signIn(String url, String phoneNumber) async{
    http.Response response = await http.
      post(url, body: {'phoneNumber': phoneNumber});

    return response.statusCode == 200;
  }

  Future<bool> signUp(String url, String phoneNumber, String displayName) async{
    http.Response response = await http.
      post(url, body: {'phoneNumber': phoneNumber, 'displayName': displayName});

    return response.statusCode == 200;
  }
}