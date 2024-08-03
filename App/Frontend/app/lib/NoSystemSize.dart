import 'package:app/api_objects/Prediction.dart';
import 'package:app/editpost.dart';
import 'package:app/editprofile.dart';
import 'package:app/homeuser.dart';
import 'package:flutter/material.dart';
import 'package:flutter/gestures.dart';

/*****************************Global variables***********************/
double scw = 0;
double sch = 0;
double t = 0;

double wid(double w) {
  return scw * (w / 460);
}

double hig(double h) {
  return (h / 680) * sch;
}

class NoSystemSize extends StatefulWidget {
  @override
  _NoSystemSizeState createState() => _NoSystemSizeState();
}

class _NoSystemSizeState extends State<NoSystemSize> {
/**************************main page************************************/
  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width; // 60% of screen width
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;

    return  WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
        return false;
      },
      child: Scaffold(
          body: Center(
              child: Column(children: <Widget>[
        SizedBox(
          height: hig(120),
          width: double.infinity,
          /**************************First Part of Screen**********************************************/
          child: Stack(
            children: [
              /*******Cloud Image********/
              Positioned(
                top: 0,
                right: 0,
                child: Image.asset(
                  'assets/Prediction/cloud.png',
                  fit: BoxFit.fill,
                  width: wid(187.5),
                  height: hig(110),
                ),
              ),
              /******arrow and title*******/
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  GestureDetector(
                    onTap: () {
                    Navigator.pushReplacement(
                        context,
                        MaterialPageRoute(builder: (context) => HomeUser(userr: user,)),
                      );
      
                    },
                    child: Image.asset(
                      'assets/Prediction/arrow.png',
                      width: wid(94),
                      height: hig(100),
                    ),
                  ),
      
                  Text(
                    'Prediction',
                    style: TextStyle(
                      fontSize: 30.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0x0ff063221),
                    ),
                  ),
                  SizedBox(
                    width: wid(94),
                  ),
                  // To take all remaining space
                ],
              ),
            ],
          ),
        ),
        /*********************warining image**********************/
        Spacer(flex: 1),
        Center(
            child: Image.asset(
          "assets/Prediction/warning.PNG",
          width: wid(350),
          height: hig(300),
        )),
        /********************Warnning message************************/
        Center(
          child: ConstrainedBox(
            constraints: BoxConstraints(
              maxWidth: wid(350),
            ),
            child: Container(
              child: Text(
                textAlign: TextAlign.center,
                "System size is missing from your profile!  Please update it and try again.",
                style: TextStyle(
                  color: Color(0xff063221),
                  fontSize: 20,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
          ),
        ),
        SizedBox(
          height: hig(10),
        ),
        /******************button go to editProfile page*********************/
        SizedBox(
          width: wid(200),
          height: hig(40),
          child: ElevatedButton(
            onPressed: () {
               Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => EditProfile()),
              );
             
            },
            child: Text(
              'Edit Profile',
              style: TextStyle(
                fontSize: 15,
                color: Color(0xff063221),
                fontWeight: FontWeight.bold,
              ),
            ),
            style: ElevatedButton.styleFrom(
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(20),
              ),
              backgroundColor: Color(0xffD9EDCA),
            ),
          ),
        ),
        Spacer(flex: 20),
      ]))),
    );
  }
}
