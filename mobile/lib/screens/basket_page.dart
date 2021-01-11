import 'package:flutter/material.dart';
import 'package:mobile/store/course_store.dart';
import 'package:provider/provider.dart';

class BasketPage extends StatefulWidget {
  @override
  _BasketPageState createState() => _BasketPageState();
}

class _BasketPageState extends State<BasketPage> {

  CourseStore courseStore;

  @override
  Widget build(BuildContext context) {
    courseStore = Provider.of<CourseStore>(context);

    return Container();
  }
}

