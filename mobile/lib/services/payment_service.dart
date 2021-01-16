import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:mobile/models/course.dart';
import 'package:mobile/models/order.dart';
import 'package:mobile/models/user.dart';

class PaymentService{

  PaymentService();

  String createOrderUrl = 'https://audioshoppp.ir/api/order';
  String payOrderUrl = 'https://audioshoppp.ir/api/payment/payorder';

  Future<String> createOrder(List<Course> basket, String userId, int totalPrice, String token) async {
    var body = jsonEncode({
      'userId': userId,
      'totalPrice': totalPrice,
      'courseDtos': basket});

    http.Response response = await http.post(Uri.encodeFull(createOrderUrl),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json",
          "Authorization": "Bearer $token",
        });

    if(response.statusCode == 200){
      String data = response.body;
      // var orderMap = jsonDecode(data);
      // Order verifiedOrder = Order.fromJson(orderMap);
      return data;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }

  Future<String> payOrder(String orderJson) async{
    http.Response response = await http.post(Uri.encodeFull(payOrderUrl),
        body: orderJson,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json",
        });

    if(response.statusCode == 302){
      String location = response.headers['location'];
      return location;
    }
    else{
      return null;
    }
  }
}