import 'dart:ui';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:mobile/screens/now_playing.dart';
import 'package:mobile/services/courses.dart';

class CoursePage extends StatefulWidget {
  CoursePage(this.courseDetails, this.courseEpisodeDetails);

  final dynamic courseDetails;
  final dynamic courseEpisodeDetails;

  @override
  _CoursePageState createState() => _CoursePageState();
}

class _CoursePageState extends State<CoursePage> {
  Widget scrollView;
  double width;
  double height;
  List<Widget> episodesList = List<Widget>();

  @override
  void initState() {
    super.initState();
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
                  flex: 2,
                  child: Image.network(
                    picUrl,
                    height: height/10,),
                ),
                Expanded(
                  flex: 6,
                  child: Padding(
                    padding: const EdgeInsets.only(right: 12.0),
                    child: Column(
                      children: <Widget>[
                        Text(episodeName),
                        Text(episodeDescription,
                            overflow: TextOverflow.ellipsis)],
                    ),
                  ),
                ),
                Expanded(
                  flex: 2,
                  child: TextButton(
                    onPressed: (){
                      Navigator.push(context, MaterialPageRoute(builder: (context){
                        return NowPlaying(episode, picUrl);
                      }));
                    },
                    child: Icon(
                        Icons.play_arrow,
                        size: 55,
                        color: Colors.deepOrange,
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
    updateUI(widget.courseDetails, widget.courseEpisodeDetails);
    return Scaffold(body: scrollView);
  }
}
