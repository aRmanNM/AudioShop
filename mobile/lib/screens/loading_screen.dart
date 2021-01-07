import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:mobile/screens/home_page.dart';
import 'package:mobile/services/courses.dart';

class LoadingScreen extends StatefulWidget {
  @override
  _LoadingScreenState createState() => _LoadingScreenState();
}

class _LoadingScreenState extends State<LoadingScreen> {
  final String url = 'https://audioshoppp.ir/api/course';

  @override
  void initState() {
    super.initState();
    getCourses();
  }

  void getCourses() async {
    CourseData courseData = CourseData(url);
    var courses = await courseData.getData();
    
    Navigator.push(context, MaterialPageRoute(builder: (context){
      return HomePage(courses);
    }));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SpinKitWave(
        color: Colors.orange,
        size: 100.0,
      ),
    );
  }
}
