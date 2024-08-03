import 'dart:async';
import 'package:app/api_objects/Prediction.dart';
import 'package:app/homeuser.dart';
import 'package:flutter/material.dart';
import 'package:flutter/gestures.dart';

/**********************Global variables***********************/
/************contain 40 hour ***************/
List<Hour> hoursList = List.generate(40, (index) {
  num start = index * 2;
  num total = start + 2;

  return Hour(start: start.toString() + "AM", total: total);
});
/***************contain 5Days*****************/
List<Day> FiveDays = [
  Day(name: "Tod", totalEnergy: 1),
  Day(name: "Sun", totalEnergy: 1),
  Day(name: "Mon", totalEnergy: 1),
  Day(name: "Thu", totalEnergy: 1),
  Day(name: "oo", totalEnergy: 1)
];

List<bool> vis = [true, false, false, false, false];
double scw = 0;
double sch = 0;
double t = 0;
int max = 1;
double columnlength = 0;
double columnwidth = 0;
double rowwidth = 0;
double rowhig = 0;

double wid(double w) {
  return scw * (w / 460);
}

double hig(double h) {
  return (h / 680) * sch;
}

class Prediction extends StatefulWidget {
  @override
  _PredictionState createState() => _PredictionState();
}

class _PredictionState extends State<Prediction> {
  bool NoError = true;
  bool _isloading = true;
  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    getPageData();
  }

/*******************Call api************************/
  Future<void> getPageData() async {
    List<Day> f = await getFiveDays();
    List<Hour> h = await getHours();
    max = await highLoad();
    /************if response not OK*************/
    if (f.length == 0 || h.length == 0) {
      setState(() {
        NoError = false;
        _isloading = false;
      });
    }
    /**********if response Ok****************/
    else {
      setState(() {
        FiveDays = f;
        FiveDays[0].name = "Tod";
        hoursList = h;
        _isloading = false;
        NoError = true;
      });
    }
  }

/***************By default values**********************/
  String selectedDay = 'Tod'; // Add selectedDay variable
  int selectedDayIndex = 0; // Add selectedDayIndex variable

/************update selected column index based on day**************************/
  void updateSelectedDay(String day) {
    setState(() {
      selectedDay = day;

      selectedDayIndex = [
        FiveDays[0].name,
        FiveDays[1].name,
        FiveDays[2].name,
        FiveDays[3].name,
        FiveDays[4].name
      ].indexOf(day);
    });
  }

/**************************main page***************************/
  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width; // 60% of screen width
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    columnlength = hig(170);
    columnwidth = wid(60); //50
    rowwidth = wid(370);
    rowhig = hig(50);

    return WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
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
                      height: hig(100),
                      width: double.infinity,
                      /****************First Part of Screen***********************/
                      child: Stack(
                        children: [
                          Positioned(
                            top: 0,
                            right: 0,
                            /****cloud image****/
                            child: Image.asset(
                              'assets/Prediction/cloud.png',
                              fit: BoxFit.fill,
                              width: wid(187.5),
                              height: hig(90),
                            ),
                          ),
                          Column(
                            children: [
                              Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                // Align items vertically
                                children: [
                                  /*****arrow*****/
                                  GestureDetector(
                                    onTap: () {
                                      Navigator.pushReplacement(
                                        context,
                                        MaterialPageRoute(
                                            builder: (context) => HomeUser(
                                                  userr: user,
                                                )),
                                      );
                                    },
                                    child: Image.asset(
                                      'assets/Prediction/arrow.png',
                                      width: wid(94),
                                      height: hig(80),
                                    ),
                                  ),
                                  /*****title******/
                                  Text(
                                    'Prediction',
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
                    /*********** Days(kw) title************/
                    Center(
                      child: SizedBox(
                        width: wid(130),
                        height: hig(35),
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.center,
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                Text(
                                  '5 days',
                                  style: TextStyle(
                                    fontSize: 25.0 * t,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0x0ffED9555),
                                  ),
                                ),
                                Text(
                                  ' (KW)',
                                  style: TextStyle(
                                    fontSize: 12.0 * t,
                                    fontWeight: FontWeight.bold,
                                    color: Color(0x0ffBBBAB6),
                                  ),
                                ),
                              ],
                            ),
                            Container(
                              height: hig(2),
                              width: wid(120),
                              color: Color(0x0ffCACBD4),
                            ),
                          ],
                        ),
                      ),
                    ),
                    /*****************Chart******************/
                    Chart(updateSelectedDay),
                    /***************grey info raw*****************/
                    Padding(
                      padding: EdgeInsets.symmetric(
                          horizontal: wid(20), vertical: hig(3)),
                      child: Row(
                        children: [
                          /*****icon*****/
                          Icon(
                            Icons.info,
                            size: 18,
                            color: Color(0x0ffBBBAB6),
                          ),
                          SizedBox(
                            width: wid(2),
                          ),
                          /***** note *******/
                          Text(
                            'Average productivity of panels per day',
                            style: TextStyle(
                              fontSize: 13.0 * t,
                              fontWeight: FontWeight.bold,
                              color: Color(0x0ffBBBAB6),
                            ),
                          ),
                        ],
                      ),
                    ),
                    /***********raw contain selected day and its avg************/
                    Padding(
                      padding: EdgeInsets.symmetric(vertical: hig(8.0)),
                      child: Row(
                        children: [
                          SizedBox(
                            width: wid(13.4),
                          ),
                          Container(
                            decoration: BoxDecoration(
                              borderRadius: BorderRadius.circular(20),
                              color: Color(0xffFB9534),
                            ),
                            width: wid(10.7),
                            height: hig(25),
                          ),
                          SizedBox(
                            width: wid(13.4),
                          ),
                          Text(
                            selectedDay, // Display the selected day
                            style: TextStyle(
                              fontSize: 20.0 * t,
                              fontWeight: FontWeight.bold,
                              color: Color(0x0ff063221),
                            ),
                          ),
                          Spacer(flex: 1),
                          Text(
                            'Avg ' +
                                FiveDays[selectedDayIndex]
                                    .totalEnergy
                                    .toString() +
                                ' KW',
                            style: TextStyle(
                              fontSize: 15.0 * t,
                              fontWeight: FontWeight.bold,
                              color: Color(0x0ff5D7A6E),
                            ),
                          ),
                          SizedBox(
                            width: wid(13.4),
                          ),
                        ],
                      ),
                    ),
                    /*********************hours of day*************************/
                    rowhours(selectedDayIndex) // Pass the selectedDayIndex
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
      ),
    );
  }
}

/*************************Chart******************************/
class Chart extends StatefulWidget {
  final Function(String)
      onDaySelected; // Callback function that change day based on click
  Chart(this.onDaySelected);

  @override
  _ChartState createState() => _ChartState();
}

class _ChartState extends State<Chart> {
  List<bool> vis = [true, false, false, false, false];
  int selectedDayIndex = 0; // Add selected day index

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: wid(330), //280
      child: Row(
        children: [
          /************if click on tod***************/
          GestureDetector(
            onTap: () {
              setState(() {
                vis = [true, false, false, false, false];
                selectedDayIndex = 0; // Update selected day index
              });
              widget.onDaySelected(
                  FiveDays[0].name); // Call the callback with day information
            },
            child: column(FiveDays[0].totalEnergy, Color(0x0ffED9555), vis[0],
                FiveDays[0].name),
          ),
          Spacer(flex: 1),
          /************if click on day2***************/
          GestureDetector(
            onTap: () {
              setState(() {
                vis = [false, true, false, false, false];
                selectedDayIndex = 1; // Update selected day index
              });
              widget.onDaySelected(
                  FiveDays[1].name); // Call the callback with day information
            },
            child: column(FiveDays[1].totalEnergy, Color(0x0ff063221), vis[1],
                FiveDays[1].name),
          ),
          Spacer(flex: 1),
          /************if click on day3***************/
          GestureDetector(
            onTap: () {
              setState(() {
                vis = [false, false, true, false, false];
                selectedDayIndex = 2; // Update selected day index
              });
              widget.onDaySelected(
                  FiveDays[2].name); // Call the callback with day information
            },
            child: column(FiveDays[2].totalEnergy, Color(0x0ff063221), vis[2],
                FiveDays[2].name),
          ),
          Spacer(flex: 1),
          /************if click on day4***************/
          GestureDetector(
            onTap: () {
              setState(() {
                vis = [false, false, false, true, false];
                selectedDayIndex = 3; // Update selected day index
              });
              widget.onDaySelected(
                  FiveDays[3].name); // Call the callback with day information
            },
            child: column(FiveDays[3].totalEnergy, Color(0x0ff063221), vis[3],
                FiveDays[3].name),
          ),
          Spacer(flex: 1),
          /************if click on day5***************/
          GestureDetector(
            onTap: () {
              setState(() {
                vis = [false, false, false, false, true];
                selectedDayIndex = 4; // Update selected day index
              });
              widget.onDaySelected(
                  FiveDays[4].name); // Call the callback with day information
            },
            child: column(FiveDays[4].totalEnergy, Color(0x0ff063221), vis[4],
                FiveDays[4].name),
          ),
        ],
      ),
    );
  }
}

/*************************build column************************/
class column extends StatelessWidget {
  TextEditingController controller = TextEditingController();
  String day;
  num precintage;
  Color color;
  bool vis;
  column(this.precintage, this.color, this.vis, this.day);
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: columnwidth,
      height: columnlength + hig(50), //50 good with 70
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Spacer(flex: 1),
            /***************write total of column*******************/
            Text(
              precintage.toString(),
              style: TextStyle(
                fontSize: 15.0 * t,
                fontWeight: FontWeight.bold,
                color: vis ? Color(0x0ffED9555) : Color(0x0ff063221),
              ),
            ),
            SizedBox(
              height: hig(1),
            ),
            /***************container with specific color*******************/
            Container(
              width: columnwidth,
              height: columnlength * (precintage / max),
              decoration: BoxDecoration(
                  borderRadius: BorderRadius.only(
                    topLeft: Radius.circular(20),
                    topRight: Radius.circular(20),
                  ),
                  color: vis ? Color(0x0ffED9555) : Color(0x0ff063221)),
            ),
            SizedBox(
              width: wid(120),
              height: hig(30),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  /***************write day of column*******************/
                  Text(
                    day,
                    style: TextStyle(
                      fontSize: 15.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Colors.black,
                    ),
                  ),
                  Visibility(
                    child: Container(
                      height: hig(3),
                      width: wid(columnwidth * 0.9),
                      color: Color(0x0ffCACBD4),
                    ),
                    visible: vis,
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/*********************hours of specific day*****************************/
class rowhours extends StatelessWidget {
  TextEditingController controller = TextEditingController();
  int selectedDayIndex = 0; // Add selected day index

  rowhours(this.selectedDayIndex); // Update the constructor

  @override
  Widget build(BuildContext context) {
    // Filter daysList based on selected day index
    /**********************Which 8 hours to take based on selected day*******************************/
    List<Hour> filteredDays =
        hoursList.sublist(selectedDayIndex * 8, (selectedDayIndex + 1) * 8);

    return Column(
      children: List.generate(filteredDays.length, (index) {
        Hour hour = filteredDays[index];

        /**************if last hour do not build divider after it****************************/
        if (index == filteredDays.length - 1) {
          return rowhour(hour.start, hour.total.toString(), "no");
        }
        /***************if not last hour build divider after it*****************************/
        else {
          Hour next = filteredDays[index + 1];
          return Column(
            children: [
              rowhour(hour.start, hour.total.toString(), "yes"),
              SizedBox(
                height: hig(5),
              )
            ],
          );
        }
      }),
    );
  }
}

/**************************build one hour raw***********************/
class rowhour extends StatelessWidget {
  TextEditingController controller = TextEditingController();
  String start;
  String power;
  String makeline;
  rowhour(this.start, this.power, this.makeline);
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: rowwidth,
      height: rowhig,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          SizedBox(
            width: rowwidth,
            height: rowhig * 0.8,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                /*****************green Container contain pridected enery of this hour***********************************/
                Container(
                  width: wid(90),
                  height: hig(rowhig * 0.8),
                  decoration: BoxDecoration(
                    borderRadius: BorderRadius.all(
                      Radius.circular(30),
                    ),
                    color: Color(0x0ffBCDFA1),
                  ),
                  child: Center(
                    child: Text(
                      power + " KW",
                      style: TextStyle(
                        fontSize: 17.0 * t,
                        fontWeight: FontWeight.bold,
                        color: Color(0x0ff5D7A6E),
                      ),
                    ),
                  ),
                ),
                SizedBox(width: wid(40)),
                /*************************from when to when****************/
                SizedBox(
                  width: wid(120),
                  child: Center(
                    child: Text(
                      start,
                      style: TextStyle(
                        fontSize: 17.0 * t,
                        fontWeight: FontWeight.bold,
                        color: Colors.black,
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
          Spacer(flex: 1),
          /*******************make divider or no*****************************/
          if (makeline == "yes") ...[
            Container(
              height: hig(3),
              width: wid(rowwidth * 0.9),
              color: Color(0x0ffCACBD4),
            ),
          ]
        ],
      ),
    );
  }
}
