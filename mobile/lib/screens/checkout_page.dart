import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:mobile/services/payment_service.dart';
import 'package:mobile/store/course_store.dart';
import 'package:provider/provider.dart';
import 'package:url_launcher/url_launcher.dart';

class CheckOutPage extends StatefulWidget {

  CheckOutPage();

  @override
  _CheckOutPageState createState() => _CheckOutPageState();
}

class _CheckOutPageState extends State<CheckOutPage> {
  CourseStore courseStore;
  PaymentService orderService = PaymentService();
  String orderJson = '';

  Future<String> createOrder() async{
    return await orderService.createOrder(
        courseStore.basket,
        courseStore.userId,
        courseStore.totalBasketPrice,
        courseStore.token
    );
  }
  @override
  Widget build(BuildContext context) {
    courseStore = Provider.of<CourseStore>(context);


    return Scaffold(
      persistentFooterButtons: [
        TextButton(
            onPressed: () async {
              orderJson = await createOrder();
              String paymentPageUrl = await orderService.payOrder(orderJson);
              if (await canLaunch(paymentPageUrl)){
                try{
                  await launch(paymentPageUrl);
                }
                catch(e){
                  print(e.toString());
                }
                finally{
                  SystemNavigator.pop();
                }
              }
              else
                Fluttertoast.showToast(msg: 'خطا در انتقال به درگاه پرداخت');
            },
            child: Text(
              'پرداخت نهایی'
            ),
        ),
      ],
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(20.0),
          child: Center(child: Text('توضیحات در مورد درگاه پرداخت')),
        ),
      ),
    );
  }
}
