import 'package:app/api_objects/Tips.dart';
import 'package:app/homeuser.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';

class TipsAndGuides extends StatefulWidget {
  /******************Questions*********************/
  final List<Question> questions = [
    Question(
        question: "What is SolarEase?",
        answer:
            "SolarEase is an application that helps you install solar systems and track predictions for your system's energy production."),
    Question(
        question: "What are the features of SolarEase?",
        answer:
            "SolarEase offers various features, including System Size Calculation, Marketplace, Chatbot, Solar Installer Finder, and Energy Production Prediction."),
    Question(
        question: "What is the Calculating feature?",
        answer:
            "This feature helps you determine the appropriate system size and its components based on your needs. It also provides information on financial savings, payback period, and environmental benefits."),
    Question(
        question: "What is the Marketplace?",
        answer:
            "The Marketplace allows users to buy and sell solar components through posts. You can also see average prices of solar components (panels, inverters, and batteries) in the real market."),
    Question(
        question: "What is the Chatbot?",
        answer:
            "The Chatbot provides answers to a variety of questions about solar energy, helping you learn more about the topic."),
    Question(
        question: "What is the Find Solar Installer feature?",
        answer:
            "This feature provides information about solar companies from nearest to farthest based on your location."),
    Question(
        question: "What is the Prediction feature?",
        answer:
            "The Prediction feature forecasts the amount of electricity your system will produce by the hour and for the next five days based on your location and system size.")
  ];

  @override
  _TipsAndGuidesState createState() => _TipsAndGuidesState();
}

/***************main widget******************/
class _TipsAndGuidesState extends State<TipsAndGuides> {
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance!.addPostFrameCallback((_) {
      // Perform any setup after the first frame is rendered
    });
  }

  final ScrollController controller = ScrollController();
  double _orangeContainerHeight = 0.0;

  @override
  Widget build(BuildContext context) {
    double scw = MediaQuery.of(context).size.width;
    double sch = MediaQuery.of(context).size.height;
    double t = MediaQuery.of(context).textScaleFactor;

    double wid(double w) => scw * (w / 310);
    double hig(double h) => (h / 592) * sch;

    return  WillPopScope(
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
                    /***************Header******************/
                    TopBar(wid: wid, hig: hig, t: t),
                    /************show questions**********/
                    ...widget.questions
                        .map((q) => Padding(
                              padding: EdgeInsets.only(bottom: hig(10)),
                              child: QuestionWidget(
                                question: q.question,
                                answer: q.answer,
                                wid: wid,
                                hig: hig,
                                t: t,
                                orangeContainerHeight: _orangeContainerHeight,
                                // Each widget will have its own key
                              ),
                            ))
                        .toList(),
                    SizedBox(height: hig(20)),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

/*****************header********************/
class TopBar extends StatelessWidget {
  final double Function(double) wid;
  final double Function(double) hig;
  final double t;

  TopBar({
    required this.wid,
    required this.hig,
    required this.t,
  });

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: hig(100),
      width: double.infinity,
      child: Stack(
        children: [
          /***************top image***************/
          Positioned(
            top: 0,
            right: 0,
            child: Image.asset(
              'assets/tipsandguids/cloud.png',
              fit: BoxFit.fill,
              width: wid(140),
              height: hig(90),
            ),
          ),
          /*****************arrow and title******************/
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
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
                  'assets/tipsandguids/arrow.png',
                  width: wid(70),
                  height: hig(80),
                ),
              ),
              Text(
                'About Us',
                style: TextStyle(
                  fontSize: 25.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0x0ff063221),
                ),
              ),
              SizedBox(width: wid(70)),
            ],
          ),
        ],
      ),
    );
  }
}

/*****************widget that build question***********************/
class QuestionWidget extends StatefulWidget {
  final String question;
  final String answer;
  final double Function(double) wid;
  final double Function(double) hig;
  final double t;
  final double orangeContainerHeight;

  const QuestionWidget({
    required this.question,
    required this.answer,
    required this.wid,
    required this.hig,
    required this.t,
    required this.orangeContainerHeight,
  });

  @override
  _QuestionWidgetState createState() => _QuestionWidgetState();
}

class _QuestionWidgetState extends State<QuestionWidget> {
  final GlobalKey _orangeContainerKey = GlobalKey();
  double _orangeContainerHeight = 0.0;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance!.addPostFrameCallback((_) {
      final RenderBox orangeContainerRenderBox =
          _orangeContainerKey.currentContext!.findRenderObject() as RenderBox;
      setState(() {
        _orangeContainerHeight = orangeContainerRenderBox.size.height;
      });
    });
  }

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        children: [
          /**********Question image***********/
          Align(
            alignment: Alignment.topLeft,
            child: Padding(
              padding: const EdgeInsets.only(left: 15.0),
              child: Image.asset(
                width: widget.wid(25),
                height: widget.hig(35),
                'assets/tipsandguids/question.png',
                fit: BoxFit.fill,
              ),
            ),
          ),
          /**************build orange and green containers**************/
          LayoutBuilder(
            builder: (context, constraints) {
              return Stack(
                children: [
                  if (_orangeContainerHeight > 0)
                    Padding(
                      padding:
                          EdgeInsets.only(top: _orangeContainerHeight * 0.5),
                      child: GreenContainer(
                        text: widget.answer,
                        wid: widget.wid,
                        hig: widget.hig,
                        t: widget.t,
                        orangeContainerHeight: _orangeContainerHeight,
                      ),
                    ),
                  SizedBox(
                    key: _orangeContainerKey,
                    child: OrangeContainer(
                      text: widget.question,
                      wid: widget.wid,
                      hig: widget.hig,
                      t: widget.t,
                    ),
                  ),
                ],
              );
            },
          ),
        ],
      ),
    );
  }
}

/****************orange container that contain question***********/
class OrangeContainer extends StatelessWidget {
  final String text;
  final double Function(double) wid;
  final double Function(double) hig;
  final double t;

  const OrangeContainer({
    required this.text,
    required this.wid,
    required this.hig,
    required this.t,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      width: wid(220),
      decoration: BoxDecoration(
        color: Color(0xffFB9534),
        borderRadius: BorderRadius.circular(20),
      ),
      child: Padding(
        padding: EdgeInsets.symmetric(
          vertical: hig(2),
          horizontal: wid(5),
        ),
        child: Text(
          text,
          style: TextStyle(
            color: Colors.white,
            fontSize: 15 * t,
          ),
        ),
      ),
    );
  }
}

/****************green container that contain answer***********/
class GreenContainer extends StatelessWidget {
  final String text;
  final double Function(double) wid;
  final double Function(double) hig;
  final double t;
  final double orangeContainerHeight;

  const GreenContainer({
    required this.text,
    required this.wid,
    required this.hig,
    required this.t,
    required this.orangeContainerHeight,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      width: wid(300),
      decoration: BoxDecoration(
        color: Color(0xffD9EDCA),
        borderRadius: BorderRadius.circular(10),
      ),
      child: Padding(
        padding: EdgeInsets.only(
          left: wid(10),
          top: orangeContainerHeight * 0.6,
          bottom: hig(2),
          right: wid(2),
        ),
        child: Text(
          text.replaceAll(';', '\n'),
          style: TextStyle(
            color: Color(0xff063221),
            fontSize: 15 * t,
          ),
        ),
      ),
    );
  }
}