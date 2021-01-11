import 'package:http/http.dart' as http;
import 'dart:convert';
import 'package:mobile/models/course_episode.dart';

class CourseEpisodeData{

  CourseEpisodeData(this.url);

  final String url;

  Future<List<CourseEpisode>> getCourseEpisodes() async{
    http.Response response = await http.get(url);
    if(response.statusCode == 200){
      String data = response.body;
      var courseEpisodeMap = jsonDecode(data);
      List<CourseEpisode> courseEpisodesList = List<CourseEpisode>();
      for(var courseEpisode in courseEpisodeMap){
        courseEpisodesList.add(CourseEpisode.fromJson(courseEpisode));
      }
      return courseEpisodesList;
    }
    else{
      print(response.statusCode);
      return null;
    }
  }
}