import 'package:http/http.dart' as http;
import 'dart:convert';

import 'package:mobile/models/course.dart';

class CourseData{

  CourseData(this.url);

  final String url;

  Future<List<Course>> getCourses() async{
    http.Response response = await http.get(url);
    if(response.statusCode == 200){
      String data = response.body;
      var courseMap = jsonDecode(data);
      List<Course> coursesList = List<Course>();
      for(var course in courseMap){
        coursesList.add(Course.fromJson(course));
      }
      return coursesList;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }
}