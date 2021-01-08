import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:mobile/screens/home_page.dart';
import 'package:mobile/services/courses.dart';

import 'course_page.dart';

// ignore: must_be_immutable
class LoadingScreen extends StatefulWidget {

  LoadingScreen(this.requestedPage, this.url);
  LoadingScreen.episodes(this.requestedPage, this.url, this.course);

  final String url;
  final String requestedPage;
  dynamic course;

  @override
  _LoadingScreenState createState() => _LoadingScreenState();
}

class _LoadingScreenState extends State<LoadingScreen> {
  String url = '';
  String requestedPage = '';
  dynamic course;

  @override
  void initState() {
    super.initState();
    url = widget.url;
    requestedPage = widget.requestedPage;
    course = widget.course;
    if(requestedPage == 'HomePage')
      getCourses();
    else if(requestedPage == 'CoursePage')
      getCourseEpisodes();
    else
      skipLoading();
  }

  void getCourses() async {
    CourseData courseData = CourseData(url);
    var courses = await courseData.getData();
    
    Navigator.push(context, MaterialPageRoute(builder: (context){
      return HomePage(courses);
    }));
  }

  void getCourseEpisodes() async{

    CourseData courseEpisodeData = CourseData(url);
    dynamic courseEpisodes = await courseEpisodeData.getData();

    // Navigator.push(context, MaterialPageRoute(builder: (context) {
    //   return CoursePage(course, courseEpisodes);
    // }));
  }

  void skipLoading(){
    Navigator.pop(context, false);
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
