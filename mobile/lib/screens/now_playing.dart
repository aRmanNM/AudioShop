import 'dart:ui';

import 'package:audioplayers/audio_cache.dart';
import 'package:audioplayers/audioplayers.dart';
import 'package:flutter/material.dart';
import 'package:flutter_cache_manager/flutter_cache_manager.dart';

class NowPlaying extends StatefulWidget {
  NowPlaying(this.episodeDetails, this.courseCoverUrl);

  final dynamic episodeDetails;
  final String courseCoverUrl;

  @override
  _NowPlayingState createState() => _NowPlayingState();
}

class _NowPlayingState extends State<NowPlaying> {
  //we will need some variables
  bool playing = false; // at the begining we are not playing any song
  IconData playBtn = Icons.play_arrow; // the main state of the play button icon

  //Now let's start by creating our music player
  //first let's declare some object
  AudioPlayer _player;
  AudioCache cache;

  Duration position = new Duration();
  Duration musicLength = new Duration();

  //we will create a custom slider

  Widget slider() {
    return Container(
        child: Slider(
            activeColor: Colors.deepOrange[600],
            inactiveColor: Colors.grey[350],
            value: position.inSeconds.toDouble(),
            max: musicLength.inSeconds.toDouble(),
            onChanged: (value) {
              seekToSec(value.toInt());
              setState(() {
                position = Duration(seconds: value.toInt());
              });
            }));
  }

  //let's create the seek function that will allow us to go to a certain position of the music
  void seekToSec(int sec) {
    Duration newPos = Duration(seconds: sec);
    _player.seek(newPos);
  }

  //Now let's initialize our player
  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _player = AudioPlayer();
    cache = AudioCache(fixedPlayer: _player);

    //now let's handle the audioplayer time

    //this function will allow you to get the music duration
    _player.durationHandler = (d) {
      setState(() {
        musicLength = d;
      });
    };

    //this function will allow us to move the cursor of the slider while we are playing the song
    _player.positionHandler = (p) {
      setState(() {
        position = p;
      });
    };
  }

  @override
  Widget build(BuildContext context) {
    dynamic episode = widget.episodeDetails;
    String courseCover = widget.courseCoverUrl;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.deepOrange[600],
        title: Text(
          episode['course'],
          style: TextStyle(
            color: Colors.white,
            fontSize: 20.0,
          ),
        ),
      ),
      //let's start by creating the main UI of the app
      body: Container(
        width: double.infinity,
        decoration: BoxDecoration(
          gradient: LinearGradient(
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
              colors: [
                Colors.deepOrange[600],
                Colors.deepOrange[600],
              ]),
        ),
        child: Container(
          decoration: BoxDecoration(
            image: DecorationImage(
              image: NetworkImage(courseCover),
              fit: BoxFit.cover,
            ),
          ),
          child: BackdropFilter(
              filter: ImageFilter.blur(sigmaX: 17.0, sigmaY: 16.0),
              child: Container(
                color: Colors.black12.withOpacity(0.3),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Expanded(
                      flex: 1,
                      child: Center(
                        child: Text(
                          episode['name'],
                          style: TextStyle(
                            color: Colors.white,
                            fontSize: 19.0,
                            fontWeight: FontWeight.w400,
                          ),
                        ),
                      ),
                    ),
                    Expanded(
                      flex: 5,
                      child: Center(
                        child: Container(
                          width: MediaQuery.of(context).size.width * 0.8,
                          height: MediaQuery.of(context).size.width * 0.8,
                          decoration: BoxDecoration(
                            image: DecorationImage(
                              image: NetworkImage(courseCover),
                              fit: BoxFit.fill,
                            ),
                          ),
                        ),
                      ),
                    ),
                    Expanded(
                      flex: 1,
                      child: Center(
                        child: Text(
                          "بیطرف",
                          style: TextStyle(
                            color: Colors.white,
                            fontSize: 19.0,
                            fontWeight: FontWeight.w600,
                          ),
                        ),
                      ),
                    ),
                    Expanded(
                      flex: 3,
                      child: Container(
                        // decoration: BoxDecoration(
                        //   color: Colors.white,
                        //   borderRadius: BorderRadius.only(
                        //     topLeft: Radius.circular(10.0),
                        //     topRight: Radius.circular(10.0),
                        //   ),
                        // ),
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          crossAxisAlignment: CrossAxisAlignment.center,
                          children: [
                            Expanded(
                                flex: 1,
                                child: Row(
                                  mainAxisAlignment:
                                      MainAxisAlignment.spaceBetween,
                                  children: [
                                    Padding(
                                      padding:
                                          const EdgeInsets.only(right: 14.0),
                                      child: Text(
                                        "${position.inMinutes}:${position.inSeconds.remainder(60)}",
                                        style: TextStyle(
                                            fontSize: 12.0,
                                            color: Colors.white),
                                      ),
                                    ),
                                    Padding(
                                      padding:
                                          const EdgeInsets.only(left: 14.0),
                                      child: Text(
                                        "${musicLength.inMinutes}:${musicLength.inSeconds.remainder(60)}",
                                        style: TextStyle(
                                            fontSize: 12.0,
                                            color: Colors.white),
                                      ),
                                    ),
                                  ],
                                )),
                            Expanded(
                              flex: 1,
                              child: slider(),
                            ),
                            Expanded(
                              flex: 3,
                              child: Row(
                                mainAxisAlignment: MainAxisAlignment.center,
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: [
                                  IconButton(
                                    iconSize: 35.0,
                                    color: Colors.white,
                                    onPressed: () {},
                                    icon: Icon(
                                      Icons.skip_next,
                                    ),
                                  ),
                                  IconButton(
                                    iconSize: 45.0,
                                    color: Colors.white,
                                    onPressed: () async {
                                      //here we will add the functionality of the play button
                                      var audioFile = await DefaultCacheManager().getSingleFile(episode['fileUrl']);
                                      if (!playing) {
                                        //now let's play the song
                                        _player.play(audioFile.path, isLocal: true);
                                        setState(() {
                                          playBtn = Icons.pause;
                                          playing = true;
                                        });
                                      } else {
                                        _player.pause();
                                        setState(() {
                                          playBtn = Icons.play_arrow;
                                          playing = false;
                                        });
                                      }
                                    },
                                    icon: Icon(
                                      playBtn,
                                    ),
                                  ),
                                  IconButton(
                                    iconSize: 35.0,
                                    color: Colors.white,
                                    onPressed: () {},
                                    icon: Icon(
                                      Icons.skip_previous,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                            Expanded(flex: 2, child: SizedBox())
                          ],
                        ),
                      ),
                    ),
                  ],
                ),
              )),
        ),
      ),
    );
  }
}
