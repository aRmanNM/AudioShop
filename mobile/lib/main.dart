import 'package:flutter/material.dart';
import 'package:mobile/screens/home_page.dart';
import 'package:mobile/screens/loading_screen.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:mobile/services/courses.dart';
import 'package:persian_fonts/persian_fonts.dart';

void main() {
  runApp(
    MaterialApp(
      localizationsDelegates: [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      supportedLocales: [
        const Locale('fa', ''),
      ],
      theme: ThemeData(
        textTheme: PersianFonts.vazirTextTheme,
        primaryColor: Colors.deepOrange[600]
      ),
      home: HomePage.basic(),
    ),
  );
}
