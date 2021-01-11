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
          primaryColor: Color(0xFF1C3C54),
          scaffoldBackgroundColor: Color(0xFF0E3E5B),
          accentColor: Color(0xFF406578),
          cardColor: Color(0xFF11465E),
          textTheme: TextTheme(
            bodyText2: TextStyle(color: Colors.white),
            bodyText1: TextStyle(color: Colors.white),
          )
        ),
        home: HomePage.basic(),
      ),
    ),
  );
}
