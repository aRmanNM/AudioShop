import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:mobile/models/course.dart';
import 'package:mobile/models/order.dart';
import 'package:mobile/models/user.dart';

class PaymentService{

  PaymentService();

  String payOrderUrl = 'https://audioshoppp.ir/api/order';

  Future<int> createOrder(List<Course> basket, String userId, int totalPrice, String token) async {
    var body = jsonEncode({
      'userId': userId,
      'totalPrice': totalPrice,
      'courseDtos': basket});

    http.Response response = await http.post(Uri.encodeFull(payOrderUrl),
        body: body,
        headers: {
          "Accept": "application/json",
          "content-type": "application/json",
          "Authorization": "Bearer $token",
        });

    if(response.statusCode == 200){
      String data = response.body;
      var orderMap = jsonDecode(data);
      Order verifiedOrder = Order.fromJson(orderMap);
      return verifiedOrder.id;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }
}