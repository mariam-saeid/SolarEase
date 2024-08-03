import 'dart:async';
import 'package:app/homeuser.dart';
import 'package:app/inputCalculation.dart';
import 'package:flutter/material.dart';
import 'package:flutter/gestures.dart';

/*******************Global Variables and Functions*************************/
double scw = 0;
double sch = 0;
double t = 0;

double wid(double w) {
  return scw * (w / 415);
}

double hig(double h) {
  return (h / 592) * sch;
}

class Results extends StatefulWidget {
  @override
  _MyHomePageState createState() => _MyHomePageState();
}

/*******************************Main widget***********************************/
class _MyHomePageState extends State<Results> {
  bool NoError = true;
  bool isloading = true;

  @override
  void initState() {
    super.initState();
    if (isexist == true) {
      setState(() {
        isloading = false;
      });
    } else {
      getsystemdata();
    }
  }

 /*********************Calculate system details*******************/
  Future<void> getsystemdata() async {

    /*************not electricity bills*****************/
    if (!inpudata.ongrid || (inpudata.ongrid && inpudata.maxOrbills == "Max")) {
      NoError = await system.getSystemInfomax();
    }
    /**************electricity bills***************/
    else {
      NoError = await system.getSystemInfoyear();
    }

    setState(() {
      isloading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width;
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;

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
                    /**************Header****************/
                    Header(NoError: NoError),
                    /**************title of system size table****************/
                    SectionTitle(title: 'System size and Cost'),
                    SizedBox(height: hig(10)),
                    /**************system size table****************/
                    SizeAnsCostTable(),
                    SizedBox(height: hig(10)),
                    /**********Sytem size final results***********/
                    InfoText(
                        title: 'System Size ' +
                            (system.totalStstemSize ?? "0") +
                            " KW"),
                    /************total cost*************/
                    InfoText(title: 'Total cost ' + system.totalcost!),
                    /***********roof space************/
                    InfoText(
                        title: 'Required Space Roof ' +
                            system.roofSpace +
                            " m\u00B2"),
                    /************finantial saving if on grid**************/
                    if (inpudata.ongrid) SectionTitle(title: 'Financial saving'),
                    if (inpudata.ongrid) SizedBox(height: hig(10)),
                    if (inpudata.ongrid) SavingTable(),
                    if (inpudata.ongrid) SizedBox(height: hig(10)),
                    if (inpudata.ongrid)
                      InfoText(
                          title: 'Payback period ' + (system.payback ?? '0')),
                    if (inpudata.ongrid)
                      InfoRow("less than 1000 kw not efficient"),
                    /***************Enviromental saving*****************/
                    SectionTitle(title: 'Environmental saving'),
                    SizedBox(height: hig(10)),
                    EnviromentalSavingTable(),
                    InfoRow("Each kWh of system reduces 0.475 kg CO2 emission."),
                    SizedBox(height: hig(10)),
                    /************buttons************/
                    buttons(),
                    SizedBox(height: hig(10)),
                  ],
                ),
              ),
            ),
            if (isloading)
              const Opacity(
                  opacity: 0.8,
                  child: ModalBarrier(dismissible: false, color: Colors.black)),
            if (isloading)
              const Center(
                child: CircularProgressIndicator(),
              )
          ],
        ),
      ),
    );
  }

  /********************reset and back buttons***************************/
  Widget buttons() {
    return Row(
      children: [
        /*********Reset button********/
        Spacer(flex: 1),
        ResetButtonWidget(set: set),
        Spacer(
          flex: 3,
        ),
        /********back buttons*********/
        BackButtonWidget(),
        Spacer(
          flex: 1,
        )
      ],
    );
  }

  /************handel reset button*************/
  set() async {
    setState(() {
      isloading = true;
    });
    NoError = await system.remove();
    setState(() {
      isloading = false;
    });
    /*   maxload = "";
    maxloadvalueon = 0;
    maxloadvalueoff = 0;
    _isEnabled = true;*/

    if (isexist) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(
            builder: (context) => HomeUser(
                  userr: user,
                )),
      );
    } else {
      Navigator.pop(context);
      Navigator.pop(context);
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(
            builder: (context) => HomeUser(
                  userr: user,
                )),
      );
    }
         maxloadvalueon = -1;
    maxloadvalueoff = -1;
  }
}

/***********************first part of page(app bar)***************************/
class Header extends StatelessWidget {
  bool NoError;
  Header({required this.NoError});
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: hig(100),
      width: double.infinity,
      child: Stack(
        children: [
          /***********cloud image********/
          Positioned(
            top: 0,
            right: 0,
            child: Image.asset(
              'assets/result/cloud.png',
              fit: BoxFit.fill,
              width: wid(187.5),
              height: hig(90),
            ),
          ),
          /*******titile and back arrow******/
          Column(
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  GestureDetector(
                    onTap: () {
                      if(isexist)
                      {
                     Navigator.pushReplacement(
                          context,
                          MaterialPageRoute(
                              builder: (context) => HomeUser(
                                    userr: user,
                                  )),
                        );
                      }
                      else{
                          Navigator.pop(context);                    
                      }
                     
                      print('Arrow tapped');
                    },
                    child: Image.asset(
                      'assets/result/arrow.png',
                      width: wid(94),
                      height: hig(80),
                    ),
                  ),
                  Text(
                    'Results',
                    style: TextStyle(
                      fontSize: 25.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0x0ff063221),
                    ),
                  ),
                  SizedBox(
                    width: wid(94),
                  ),
                ],
              ),
              /************if response generate error***************/
              if (!NoError)
                Center(
                  child: Text(
                    "Oops! Something went wrong.",
                    style: TextStyle(fontSize: 15.0, color: Colors.red),
                  ),
                )
            ],
          ),
        ],
      ),
    );
  }
}

/***********************widget build title before any table*****************************/
class SectionTitle extends StatelessWidget {
  final String title;

  SectionTitle({required this.title});

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        SizedBox(width: wid(13.4)),
        Container(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(20),
            color: Color(0xffFB9534),
          ),
          width: wid(10.7),
          height: hig(25),
        ),
        SizedBox(width: wid(13.4)),
        Text(
          title,
          style: TextStyle(
            fontSize: 15.0 * t,
            fontWeight: FontWeight.bold,
            color: Color(0x0ff063221),
          ),
        ),
      ],
    );
  }
}

/************************widget for (system size),(total cost),..*********************************/
class InfoText extends StatelessWidget {
  final String title;
  InfoText({required this.title});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text(
          title,
          style: TextStyle(
            fontSize: 15.0 * t,
            fontWeight: FontWeight.bold,
            color: Color(0xffBCDFA1),
          ),
        ),
        SizedBox(height: hig(2)),
      ],
    );
  }
}

/***************************widget for grey info text******************************/
class InfoRow extends StatelessWidget {
  String s =
      'Each kilowatt-hour of solar energy results in an estimated reduction of 0.45-0.5 kg of CO2 emissions';
  InfoRow(this.s);
  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        SizedBox(width: wid(27)),
        Icon(
          Icons.info,
          color: Color(0xffBBBAB6),
          size: 18 * t,
        ),
        SizedBox(width: wid(10.7)),
        Text(
          s,
          style: TextStyle(
            color: Color(0xffBBBAB6),
            fontSize: 13 * t,
          ),
        ),
      ],
    );
  }
}

/**************************widget for back button*****************************/
class BackButtonWidget extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.only(left: wid(13.4)),
      child: Align(
        alignment: Alignment.bottomLeft,
        child: ElevatedButton(
          onPressed: () {
            if (isexist) {
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(
                    builder: (context) => HomeUser(
                          userr: user,
                        )),
              );
            } else {
              Navigator.pop(context);
              Navigator.pop(context);
              Navigator.pushReplacement(
                context,
                MaterialPageRoute(
                    builder: (context) => HomeUser(
                          userr: user,
                        )),
              );
            }
          },
          style: ElevatedButton.styleFrom(
            foregroundColor: Colors.white,
            backgroundColor: Color(0xffFB9534),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(30.0),
            ),
            minimumSize: Size(wid(162), hig(40)),
          ),
          child: Text(
            'Back Home',
            style: TextStyle(
              fontSize: 10.0 * t,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
        ),
      ),
    );
  }
}

/***********************widget for delet(reset) button***************************/
class ResetButtonWidget extends StatelessWidget {
  ResetButtonWidget({required this.set});
  VoidCallback set;
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.only(left: wid(13.4)),
      child: Align(
        alignment: Alignment.bottomLeft,
        child: ElevatedButton(
          onPressed: set,
          style: ElevatedButton.styleFrom(
            foregroundColor: Colors.white,
            backgroundColor: Colors.red,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(30.0),
            ),
            minimumSize: Size(wid(162), hig(40)),
          ),
          child: Text(
            'Reset',
            style: TextStyle(
              fontSize: 10.0 * t,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
        ),
      ),
    );
  }
}

/*******************widget build table of system components with its details*************************/
class SizeAnsCostTable extends StatelessWidget {
  TextEditingController controller = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: wid(336),
      child: Container(
        height: hig(150),
        width: wid(336),
        decoration: BoxDecoration(
          color: Color(0xffECECEC),
          borderRadius: BorderRadius.all(
            Radius.circular(15.0),
          ),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            /**********Row for panels************/
            InfoRowWidget(
                title: 'Panel',
                capacity: system.panel.capacity,
                numbers: system.panel.numm,
                cost: system.panel.cost),
            Divider(height: 1, color: Colors.grey),
            /**********Row for inverters************/
            InfoRowWidget(
                title: 'Inverter',
                capacity: system.inverter.capacity,
                numbers: system.inverter.numm,
                cost: system.inverter.cost),
            Divider(height: 1, color: Colors.grey),
            /**********Row for battaries************/
            InfoRowWidget(
                title: 'Battery',
                capacity: system.battery.capacity,
                numbers: system.battery.numm,
                cost: system.battery.cost),
          ],
        ),
      ),
    );
  }
}

/****************widget build each raw in table************************/
class InfoRowWidget extends StatelessWidget {
  final String title;
  final String capacity;
  final String numbers;
  final String cost;

  InfoRowWidget(
      {required this.title,
      required this.capacity,
      required this.numbers,
      required this.cost});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: [
        SizedBox(width: wid(10)),
        /**********title *************/
        SizedBox(width: wid(60), child: Text(title)),
        SizedBox(width: wid(30)),
        /************Components details (numbers,capacity and cost)*************/
        SizedBox(
          width: wid(150),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              /**********Capacity************/
              Text(
                'Capacity: $capacity',
                style: TextStyle(
                  fontSize: 10.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0xff063221),
                ),
              ),
              /***********Numbers***********/
              Text(
                'Numbers: $numbers',
                style: TextStyle(
                  fontSize: 10.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0xff063221),
                ),
              ),
              /************cost**********/
              Text(
                'Cost: $cost',
                style: TextStyle(
                  fontSize: 10.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0xff063221),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
}

/******************widget build Finataial saving table*******************/
class SavingTable extends StatelessWidget {
  TextEditingController controller = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: hig(150),
      width: wid(336),
      child: Container(
        height: hig(150),
        width: wid(336),
        decoration: BoxDecoration(
          color: Color(0xffECECEC),
          borderRadius: BorderRadius.all(
            Radius.circular(15.0),
          ),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            /*********Saving Monthly**********/
            SavingRow(title: 'Monthly', amount: system.financial.monthly),
            Divider(height: 1, color: Colors.grey),
            /**********Saving Yearly************/
            SavingRow(title: 'Yearly', amount: system.financial.yearly),
            Divider(height: 1, color: Colors.grey),
            /***********Saving 25 Year**********/
            SavingRow(title: '25 Year', amount: system.financial.year25),
          ],
        ),
      ),
    );
  }
}

/********************widget build enviromintal saving table***********************/
class EnviromentalSavingTable extends StatelessWidget {
  TextEditingController controller = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: hig(150),
      width: wid(336),
      child: Container(
        height: hig(150),
        width: wid(336),
        decoration: BoxDecoration(
          color: Color(0xffECECEC),
          borderRadius: BorderRadius.all(
            Radius.circular(15.0),
          ),
        ),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            Text(
              'Reduce Carbon Emission',
              style: TextStyle(
                fontSize: 13.0 * t,
                fontWeight: FontWeight.bold,
                color: Color(0xffF2AE2F),
              ),
            ),
            /*********Saving Monthly**********/
            SavingRow(title: 'Monthly', amount: system.enviromental.monthly),
            Divider(height: 1, color: Colors.grey),
            /*********Saving Yearly**********/
            SavingRow(title: 'Yearly', amount: system.enviromental.yearly),
            Divider(height: 1, color: Colors.grey),
            /*********Saving 25 year**********/
            SavingRow(title: '25 Year', amount: system.enviromental.year25),
          ],
        ),
      ),
    );
  }
}

/*********************** widget build each raw in finantial or enviromental saving************************/
class SavingRow extends StatelessWidget {
  final String title;
  final String amount;

  SavingRow({required this.title, required this.amount});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: [
        SizedBox(
          width: wid(10),
        ),
        /*********title**********/
        SizedBox(width: wid(60), child: Text(title)),
        SizedBox(width: wid(60)),
        SizedBox(
          width: wid(100),
          /********amount of saving********/
          child: Text(
            amount,
            style: TextStyle(
              fontSize: 10.0 * t,
              fontWeight: FontWeight.bold,
              color: Color(0xff063221),
            ),
          ),
        ),
      ],
    );
  }
}
