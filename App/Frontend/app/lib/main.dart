import 'package:app/homeuser.dart';
import 'package:app/swapPage.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';

double scw = 0; // 60% of screen width
double sch = 0;
double t = 0;
double width(double w){return scw * (w /783);}
double height(double h){return (h / 393) * sch;}
void main() {
  runApp(const Solar());
}

class Solar extends StatelessWidget {
  const Solar({super.key});

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width; // 60% of screen width
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    return MaterialApp(
      
      debugShowCheckedModeBanner: false,
      home: WillPopScope(
          onWillPop: () async {
            // Return false to disable the back button
            return false;
          },
        child: SwapPaget()),
    );
  }
}
//   -----------------------------------------------------
