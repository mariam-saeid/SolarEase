import 'dart:async';
import 'package:app/Prediction.dart';
import 'package:app/api_objects/Message.dart';
import 'package:app/api_objects/Prediction.dart';
import 'package:app/homeuser.dart';
import 'package:app/profile.dart';
import 'package:flutter/material.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/widgets.dart';

/*************Global Variables**************/
double scw = 0;
double sch = 0;
double t = 0;

double wid(double w) {
  return scw * (w / 460);
}

double hig(double h) {
  return (h / 680) * sch;
}

class Messages extends StatefulWidget {
  @override
  _MessagesState createState() => _MessagesState();
}

/**************************main widget*************************/
class _MessagesState extends State<Messages> {
  bool NoError = true;
  List<Message> mesageslist = [];
  bool _isloading = true;
  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    getMessages();
  }
/****************api for Messages********************/
  Future<void> getMessages() async {
    NoError = await allMessages(mesageslist);
    setState(() {
      _isloading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width; // 60% of screen width
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    return  WillPopScope(
      onWillPop: () async {
        return false;
      },
      child: Scaffold(
        body: Stack(
          children: [
            SingleChildScrollView(
              child: Center(
                child: Column(
                  children: <Widget>[
                    SizedBox(
                      height: hig(120),
                      width: double.infinity,
                      /***********************First Part of Screen***********************************/
                      child: Stack(
                        children: [
                          /*******cloud image********/
                          Positioned(
                            top: 0,
                            right: 0,
                            child: Image.asset(
                              'assets/Prediction/cloud.png',
                              fit: BoxFit.fill,
                              width: wid(187.5),
                              height: hig(120),
                            ),
                          ),
                          /*******page title********/
                          Column(
                            children: [
                              Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                // Align items vertically
                                children: [
                                  SizedBox(
                                    height: hig(100),
                                    width: wid(94),
                                  ),
                                  // Add space between the icon and text
                                  Text(
                                    'Messages',
                                    style: TextStyle(
                                      fontSize: 35.0 * t,
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
                              /********if api return error *******/
                              if (!NoError)
                                Center(
                                  child: Text(
                                    "Oops! Something went wrong.",
                                    style: TextStyle(
                                        fontSize: 15.0, color: Colors.red),
                                  ),
                                )
                            ],
                          ),
                        ],
                      ),
                    ),
                    /*******if no message******/
                    if (mesageslist.length == 0) emptymessage(),
      
                    /*******if there zare messages******/
                    if (mesageslist.length > 0) MessagesUser(m: mesageslist),
                  ],
                ),
              ),
            ),
            if (_isloading)
              const Opacity(
                  opacity: 0.5,
                  child: ModalBarrier(dismissible: false, color: Colors.black)),
            if (_isloading)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
        bottomNavigationBar: BottomNavBar(),
      ),
    );
  }
}

/*******************build page when no message *************************/
class emptymessage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        /*********NoMessage image*********/
        Center(
            child: Image.asset(
          "assets/Message/NoMessage.png",
          width: wid(370),
          height: hig(300),
        )),
        /*********warning message***********/
        Center(
          child: ConstrainedBox(
            constraints: BoxConstraints(
              maxWidth: wid(350),
            ),
            child: Container(
              child: Text(
                textAlign: TextAlign.center,
                "There is No Messages",
                style: TextStyle(
                  color: Color(0xffFB9535),
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
      ],
    );
  }
}

/*******************build page if there are messages****************************/
class MessagesUser extends StatelessWidget {
  final List<Message> m;
  MessagesUser({required this.m});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: m.map((message) {
        return Column(
          children: <Widget>[
            Container(
              width: wid(450),
              decoration: BoxDecoration(
                color: Color(0xffDDEFD0),
                borderRadius: BorderRadius.circular(10),
              ),
              child: Padding(
                padding: const EdgeInsets.all(8.0),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    /******Title*****/
                    Text(
                      message.title,
                      style: TextStyle(
                          fontSize: 15,
                          color: Color(0x0ffFB9535),
                          fontWeight: FontWeight.bold),
                    ),
                    /*******Body******/
                    Text(
                      message.body,
                      style: TextStyle(color: Color(0x0ff063221)),
                    ),
                    SizedBox(
                      height: hig(5), // Replace hig(10) with 10
                    ),
                    /******date******/
                    Align(
                      alignment: Alignment.centerRight,
                      child: Text(
                        textAlign: TextAlign.right,
                        message.time,
                        style: TextStyle(color: Color(0x0ff063221)),
                      ),
                    ),
                  ],
                ),
              ),
            ),
            SizedBox(
              height: 10, // Replace hig(10) with 10
            ),
          ],
        );
      }).toList(),
    );
  }
}


/*********************widget for bottom navigation bar*************************/
class BottomNavBar extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
        color: Color(0xffECECEC),
        borderRadius: BorderRadius.vertical(
          top: Radius.circular(20),
        ),
      ),
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 20),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            /*********message page***********/
            _buildIconItem(Icons.message, 'Message', () {}),
            /*********Home page***********/
            _buildIconItem(Icons.home, 'Home', () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => HomeUser(
                        userr: user,
                      ) ),
              );

             
            }),
            /*********profile page***********/
            _buildIconItem(Icons.person, 'Profile', () {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(builder: (context) => ProfilePage()),
              );

              // Add onTap functionality for Profile
            }),
          ],
        ),
      ),
    );
  }

/*******************widget for each item in navigation bar***************************/
  Widget _buildIconItem(IconData iconData, String text, VoidCallback onTap) {
    return GestureDetector(
      onTap: onTap,
      child: SizedBox(
        height: hig(65),
        child: Center(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              /******icon******/
              Icon(
                iconData,
                size: 30,
                color: text == "Message"
                    ? Color.fromARGB(136, 6, 50, 33)
                    : Color(0xff063221),
              ),
              SizedBox(height: 3),
              /*****name****/
              Text(
                text,
                style: TextStyle(
                  fontSize: 10,
                  fontWeight: FontWeight.bold,
                  color: Color(0xff063221),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
