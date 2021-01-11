import 'dart:ui';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:mobile/models/course.dart';
import 'package:mobile/models/course_episode.dart';
import 'package:mobile/screens/now_playing.dart';
import 'package:mobile/services/course_episode_service.dart';
import 'package:mobile/store/course_store.dart';
import 'package:provider/provider.dart';

class CoursePage extends StatefulWidget {
  CoursePage(this.courseDetails, this.courseCover);

  final Course courseDetails;
  final courseCover;

  @override
  _CoursePageState createState() => _CoursePageState();
}

class _CoursePageState extends State<CoursePage> {
  Widget scrollView;
  double width;
  double height;
  List<Widget> episodesList = List<Widget>();
  Future<dynamic> episodesFuture;
  String url = 'https://audioshoppp.ir/api/course/episodes/';
  CourseStore courseStore;

  @override
  void initState() {
    super.initState();
    episodesFuture = getCourseEpisodes();
  }

  Future<List<CourseEpisode>> getCourseEpisodes() async{
    url += widget.courseDetails.id.toString();
    CourseEpisodeData courseEpisodeData = CourseEpisodeData(url);
    List<CourseEpisode> courseEpisodes = await courseEpisodeData.getCourseEpisodes();

    if(courseEpisodes != null)
      await updateUI(widget.courseDetails, courseEpisodes);

    return courseEpisodes;
  }

  Future updateUI(Course course, List<CourseEpisode> episodes) async {
    episodesList = List<Widget>();
    for (var episode in episodes) {
      String picUrl = course.pictureUrl;
      String episodeName = episode.name;
      String episodeDescription = episode.description;
      var picFile = widget.courseCover;

      episodesList.add(Padding(
        padding: const EdgeInsets.fromLTRB(8,8,8,0),
        child: Column(
          children: <Widget>[
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: <Widget>[
                Expanded(
                  flex: 6,
                  child: Image.file(
                    picFile,
                    height: height/10,),
                ),
                Expanded(
                  flex: 25,
                  child: Padding(
                    padding: const EdgeInsets.only(right: 12.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        Text(
                          episodeName,
                          style: TextStyle(fontSize: 16),),
                        Text(
                          episodeDescription,
                          overflow: TextOverflow.ellipsis,
                          style: TextStyle(fontSize: 12),)],
                    ),
                  ),
                ),
                Expanded(
                  flex: 4,
                  child: TextButton(
                    onPressed: (){
                      Navigator.push(context, MaterialPageRoute(builder: (context){
                        return NowPlaying(episode, picUrl);
                      }));
                    },
                    child: Icon(
                        Icons.play_arrow,
                        size: 32,
                        color: Colors.white,
                    ),
                  ),
                )
              ],
            ),
            SizedBox(
              width: width * 0.89,
              child: Divider(
                color: Colors.black26,
              ),
            )
          ]
        ),
      ));
    }

    scrollView = CustomScrollView(
      slivers: <Widget>[
        SliverAppBar(
          // floating: false, pinned: false, snap: false,
          backgroundColor: Colors.transparent,
          //title: Text(course['name']),
          expandedHeight: height / 5,
          flexibleSpace: FlexibleSpaceBar(
            background: Container(
              decoration: BoxDecoration(
                image: DecorationImage(
                  image: FileImage(widget.courseCover),
                  fit: BoxFit.fill,
                ),
              ),
              child: BackdropFilter(
                filter: ImageFilter.blur(sigmaX: 17.0, sigmaY: 16.0),
                child: Container(
                  color: Colors.black12.withOpacity(0.3),
                ),
              ),
            ),
            title: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.start,
              textBaseline: TextBaseline.alphabetic,
              children: <Widget>[
                Expanded(
                  flex: 3,
                  child: Padding(
                    padding: const EdgeInsets.fromLTRB(8.0, 8.0, 0, 0),
                    child: Image.file(
                      widget.courseCover,
                      width: width / 6,
                    ),
                  ),
                ),
                Expanded(
                  flex: 4,
                  child: Padding(
                    padding: const EdgeInsets.only(left: 10.0, top: 10.0),
                    child: Text(
                      course.name + '  -  ' + course.description,
                      style: TextStyle(fontSize: 14),
                    ),
                  ),
                ),
                Expanded(
                  flex: 2,
                    child: TextButton(
                      onPressed: (){
                        if(courseStore.addCourseToBasket(course))
                          Fluttertoast.showToast(msg: 'دوره با موفقیت به سبد خرید اضافه شد');
                        else
                          Fluttertoast.showToast(msg: 'این دوره در سبد خرید شما موجود است');
                      },
                      child: Icon(
                        Icons.shopping_basket,
                        size: 22,
                        color: Colors.white,
                      ),
                    ),)
              ],
            ),
          ),
        ),
        SliverList(
          delegate: SliverChildBuilderDelegate(
            (context, index) => Container(
              child: episodesList[index],
            ),
            childCount: episodesList.length,
          ),
        )
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    courseStore = Provider.of<CourseStore>(context);
    width = MediaQuery.of(context).size.width;
    height = MediaQuery.of(context).size.height;
    return FutureBuilder(
        future: episodesFuture,
        builder: (context, data){
          if(data.hasData){
            return Scaffold(body: scrollView);
          }
          else{
            return Container(
              color: Colors.white,
              child: SpinKitWave(
                color: Colors.deepOrange[600],
                size: 100.0,
              ),
            );
          }
        }
    );
  }
}