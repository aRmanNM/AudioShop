import 'dart:ui';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:mobile/screens/now_playing.dart';
import 'package:mobile/services/courses.dart';

class CoursePage extends StatefulWidget {
  CoursePage(this.courseDetails);

  final dynamic courseDetails;

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

  @override
  void initState() {
    super.initState();
    episodesFuture = getCourseEpisodes();
  }

  Future<dynamic> getCourseEpisodes() async{
    url += widget.courseDetails['id'].toString();
    CourseData courseEpisodeData = CourseData(url);
    var courseEpisodes = await courseEpisodeData.getData();

    if(courseEpisodes != null)
      updateUI(widget.courseDetails, courseEpisodes);

    return courseEpisodes;
  }

  void updateUI(dynamic course, dynamic episodes) {
    episodesList = List<Widget>();
    for (var episode in episodes) {
      String picUrl = course['pictureUrl'];
      String episodeName = episode['name'];
      String episodeDescription = episode['description'];
      episodesList.add(Padding(
        padding: const EdgeInsets.fromLTRB(8,8,8,0),
        child: Column(
          children: <Widget>[
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: <Widget>[
                Expanded(
                  flex: 6,
                  child: Image.network(
                    picUrl,
                    height: height/10,),
                ),
                Expanded(
                  flex: 25,
                  child: Padding(
                    padding: const EdgeInsets.only(right: 12.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        Text(episodeName),
                        Text(episodeDescription,
                            overflow: TextOverflow.ellipsis)],
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
                        color: Colors.deepOrange[600],
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
                  image: NetworkImage(course['pictureUrl']),
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
              children: <Widget>[
                Padding(
                  padding: const EdgeInsets.fromLTRB(8.0, 8.0, 0, 0),
                  child: Image.network(
                    course['pictureUrl'],
                    width: width / 6,
                  ),
                ),
                Expanded(
                  child: Text(
                    course['name'] + '  -  ' + course['description'],
                    style: TextStyle(fontSize: 14),
                  ),
                ),
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
