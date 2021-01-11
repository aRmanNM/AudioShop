import 'package:flutter/material.dart';
import 'package:mobile/screens/home_page.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:mobile/store/course_store.dart';
import 'package:provider/provider.dart';

void main() {
  runApp(
    ChangeNotifierProvider(
      create: (context) => CourseStore(),
      child: MaterialApp(
        debugShowCheckedModeBanner: false,
        localizationsDelegates: [
          GlobalMaterialLocalizations.delegate,
          GlobalWidgetsLocalizations.delegate,
          GlobalCupertinoLocalizations.delegate,
        ],
        supportedLocales: [
          const Locale('fa', ''),
        ],
        theme: ThemeData(
          fontFamily: 'IranSans',
          primaryColor: Colors.deepOrange[600]
        ),
        home: HomePage.basic(),
      ),
    ),
  );
}
