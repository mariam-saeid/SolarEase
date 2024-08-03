import 'package:app/inputCalculation.dart';
import 'package:app/result.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

/**********************Global variables and functions**************************/
double scw = 0; // 60% of screen width
double sch = 0;
double t = 5;

_wid(var w) {
  return (scw * (w / 440));
}

_hig(var h) {
  return (h / 592) * sch;
}

class Maxload extends StatefulWidget {
  @override
  _MaxloadState createState() => _MaxloadState();
}

//////////////////////////////////////////
class _MaxloadState extends State<Maxload> {
  String maxload = "-1";
  bool clickednext = false;
  /*****************main devices******************/
  List<Device> devices = [
    Device(name: "Air conditioning", waat: 1350, numbers: 0),
    Device(name: "Wall fan", waat: 65, numbers: 0),
    Device(name: "Vertical fan", waat: 100, numbers: 0),
    Device(name: "broom", waat: 1600, numbers: 0),
    Device(name: "laptop", waat: 100, numbers: 0),
    Device(name: "Lamp", waat: 15, numbers: 0),
    Device(name: "lcd TV", waat: 100, numbers: 0),
    Device(name: "charger", waat: 7, numbers: 0),
    Device(name: "Router", waat: 10, numbers: 0),
    Device(name: "landline", waat: 5, numbers: 0),
    Device(name: "Freezer", waat: 300, numbers: 0),
    Device(name: "Fridge", waat: 500, numbers: 0),
    Device(name: "washing machine", waat: 500, numbers: 0),
    Device(name: "water cooler", waat: 600, numbers: 0),
    Device(name: "Microwave", waat: 1700, numbers: 0),
    Device(name: "Cattail", waat: 1100, numbers: 0),
    Device(name: "Iron", waat: 1000, numbers: 0),
  ];

  late TextEditingController maxloadcontroller;
  @override
  void initState() {
    super.initState();
    maxloadcontroller =
        TextEditingController(text: maxload != '-1' ? maxload : '');
  }

  /**************************main widget****************************/
  @override
  Widget build(BuildContext context) {
    scw = MediaQuery.of(context).size.width; // 60% of screen width
    sch = MediaQuery.of(context).size.height;
    t = MediaQuery.of(context).textScaleFactor;
    double totalLoad =
        devices.fold(0, (sum, device) => sum + device.waat * device.numbers);

    return  WillPopScope(
      onWillPop: () async {
        // Return false to disable the back button
        return false;
      },


      child: Scaffold(
        body: SingleChildScrollView(
          child: Center(
            child: Column(
              children: [
                /***************Header*****************/
                HeaderWidget(),
                SizedBox(height: _hig(20)),
                /****************Maxload Title******************/
                MaxLoadInputWidget(),
                SizedBox(height: _hig(20)),
                /****************Maxload Input***********************/
                MaxLoadcontainer(
                  maxload: 'Devices Load',
                  onChanged: (String value) {
                    setState(() {
                      maxload = value;
                      print(maxload);
                    });
                  },
                  initialValue: maxload,
                  controler: maxloadcontroller,
                ),
                /*****************if input empty or zero or -1 and click ***************************/
                (maxload == '' || double.tryParse(maxload)! <= 0) && clickednext
                    ? builderrorinputtext()
                    : SizedBox(height: _hig(20)),
                /**********************Grey info***********************/
                InfoRowWidget(),
                SizedBox(height: _hig(5)),
                /**********************Devices table********************************/
                DevicesDataTable(devices: devices, totalload: totalLoad),
                SizedBox(height: _hig(20)),
                /******************Add Divice Button**********************/
                AddDeviceButton(onPressed: _addDevice),
                SizedBox(height: _hig(20)),
                /*********************Result of table*********************************/
                TotalLoadWidget(totalLoad: totalLoad),
                SizedBox(height: _hig(20)),
                /***********************Next Button***************************/
                NextButtonWidget(onPressed: () {
                  setState(() {
                    clickednext = true;
                  });
                  if ((double.tryParse(maxload) ?? 0) > 0) {
                    inpudata.maxload = double.tryParse(maxload) ?? 0.0;
                    // Navigator.pushReplacement(
                    //   context,
                    //   MaterialPageRoute(builder: (context) => ),
                    // );
                     Navigator.of(context)
                        .push(MaterialPageRoute(builder: (_) => Results()));
                  }
      
                  print("inpudata.elecitycoverage" +
                      inpudata.elecitycoverage.toString());
                  print("inpudata.maxOrbills" + inpudata.maxOrbills);
                  print("input maxload " + inpudata.maxload.toString());
                  print("input maxusageoff " + inpudata.maxusageoff.toString());
                  print("input maxusageon " + inpudata.maxusageon.toString());
                }),
              ],
            ),
          ),
        ),
      ),
    );
  }

  /********************function to add device in table***********************/
  void _addDevice() {
    setState(() {
      devices.add(Device(name: "New Device", waat: 0, numbers: 0));
    });
  }

  /***********************widget to build error message*************************/
  Widget builderrorinputtext() {
    /*******empty data*******/
    if (maxload == '' || double.tryParse(maxload)! < 0)
      return Text(
        "Please Enter Value",
        style: TextStyle(color: Colors.red, fontSize: 13),
      );
    /*******input = 0*******/
    else {
      return Text(
        "Please Enter Value more than 0",
        style: TextStyle(color: Colors.red, fontSize: 13),
      );
    }
  }
}

/***************************widget of first part of page*****************************/
class HeaderWidget extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: _hig(100),
      width: double.infinity,
      child: Stack(
        children: [
          /*********cloud image*********/
          Positioned(
            top: 0,
            right: 0,
            child: Image.asset(
              'assets/Maxload/cloud.png',
              fit: BoxFit.fill,
              width: _wid(200),
              height: _hig(90),
            ),
          ),
          /*********arrow and title*********/
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              GestureDetector(
                onTap: () {
                  Navigator.pop(context);
                  // Navigator.pushReplacement(
                  //   context,
                  //   MaterialPageRoute(builder: (context) => InputCalculations()),
                  // );

                  print('Arrow tapped');
                },
                child: Image.asset(
                  'assets/Maxload/arrow.png',
                  width: _wid(100),
                  height: _hig(100),
                ),
              ),
              Text(
                'Devices Load',
                style: TextStyle(
                  fontSize: 25.0 * t,
                  fontWeight: FontWeight.bold,
                  color: Color(0xff063221),
                ),
              ),
              SizedBox(
                width: _wid(70),
              ),
            ],
          ),
        ],
      ),
    );
  }
}

/************************widget that build "max Load" text****************************/
class MaxLoadInputWidget extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: _wid(150),
      height: _hig(30),
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              'Devices Load',
              style: TextStyle(
                fontSize: 20.0 * t,
                fontWeight: FontWeight.bold,
                color: Color(0xff063221),
              ),
            ),
            Spacer(flex: 1),
            Container(
              width: _wid(150),
              height: _hig(3),
              color: Color(0xffCACBD4),
            )
          ],
        ),
      ),
    );
  }
}

/****************************widget for grey text (info)*******************************/
class InfoRowWidget extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        SizedBox(width: _wid(15)),
        Icon(
          Icons.info,
          color: Color(0xffBBBAB6),
          size: 18 * t,
        ),
        SizedBox(width: _wid(10.7)),
        Text(
          'This table helps you calculate max load. It is optional.',
          style: TextStyle(
            color: Color(0xffBBBAB6),
            fontSize: 13 * t,
          ),
        ),
      ],
    );
  }
}

/////////////////////////////////////////////////
class DevicesDataTable extends StatefulWidget {
  final List<Device> devices;
  double totalload;
  DevicesDataTable({required this.devices, required this.totalload});

  @override
  State<DevicesDataTable> createState() => _DevicesDataTableState();
}

/************************widget to build table*********************************/
class _DevicesDataTableState extends State<DevicesDataTable> {
  /********Recalculate total load when values in the table change*********/
  void _updateTotalLoad() {
    setState(() {
      widget.totalload = widget.devices
          .fold(0, (sum, device) => sum! + device.waat * device.numbers);
    });
  }

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      child: ClipRRect(
        borderRadius: BorderRadius.circular(10),
        child: Container(
          constraints: BoxConstraints(minWidth: _wid(420)),
          decoration: BoxDecoration(
            color: Colors.grey[200],
          ),
          child: DataTable(
            headingRowColor:
                MaterialStateColor.resolveWith((states) => Color(0xffD9EDCA)),
            columnSpacing: _wid(16),
            columns: [
              /**********Device name *************/
              DataColumn(
                label: Text(
                  'Device',
                  style: TextStyle(
                    fontSize: 17.0 * t,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
              ),
              /************Device watt************/
              DataColumn(
                label: Text(
                  'Watt',
                  style: TextStyle(
                    fontSize: 17.0 * t,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
              ),
              /**********Device numbers**********/
              DataColumn(
                label: Text(
                  'Numbers',
                  style: TextStyle(
                    fontSize: 17.0 * t,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
              ),
              /**********total**********/
              DataColumn(
                label: Text(
                  'Total',
                  style: TextStyle(
                    fontSize: 17.0 * t,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
              ),
            ],
            rows: widget.devices.map((device) {
              double total = device.waat * device.numbers;
              return DataRow(cells: [
                /*********Device name (ex:TV)********/
                DataCell(
                  Text(
                    device.name,
                    style: TextStyle(
                      fontSize: 15.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0xff063221),
                    ),
                  ),
                ),
                /*********Device waat(Ex:100)*******/
                DataCell(
                  Container(
                    height: _hig(35),
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(10),
                    ),
                    padding: EdgeInsets.symmetric(
                        horizontal: _wid(8), vertical: _hig(1)),
                    child: Center(
                      child: TextFormField(
                        textAlign: TextAlign.center,
                        initialValue:
                            device.waat.toString(), // Set initialValue here
                        keyboardType: TextInputType.number,
                        onChanged: (value) {
                          device.waat = double.parse(value);
                          _updateTotalLoad();
                        },
                        decoration: InputDecoration(
                          border: InputBorder.none,
                        ),
                      ),
                    ),
                  ),
                ),
                /**********Device Numbers(Ex:1)**********/
                DataCell(
                  Container(
                    height: _hig(35),
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(10),
                    ),
                    padding: EdgeInsets.symmetric(
                        horizontal: _wid(8), vertical: _hig(1)),
                    child: Center(
                      child: TextFormField(
                        textAlign: TextAlign.center,
                        initialValue: device.numbers.toString(),
                        keyboardType: TextInputType.number,
                        onChanged: (value) {
                          device.numbers = int.parse(value);
                          _updateTotalLoad();
                        },
                        decoration: InputDecoration(
                          border: InputBorder.none,
                        ),
                      ),
                    ),
                  ),
                ),
                /************total***********/
                DataCell(
                  Container(
                    height: _hig(35),
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(10),
                    ),
                    padding: EdgeInsets.symmetric(
                        horizontal: _wid(8), vertical: _hig(8)),
                    child: Center(
                      child: Text(
                        total.toString(),
                        style: TextStyle(color: Colors.black),
                      ),
                    ),
                  ),
                )
              ]);
            }).toList(),
          ),
        ),
      ),
    );
  }
}

/**********************widget to build "add device" botton***********************************/
class AddDeviceButton extends StatelessWidget {
  final VoidCallback onPressed;
  AddDeviceButton({required this.onPressed});

  @override
  Widget build(BuildContext context) {
    return Align(
      alignment: Alignment.centerRight,
      child: Container(
        width: _wid(40),
        height: _hig(30),
        decoration: BoxDecoration(
          color: Color(0xffED9555),
          borderRadius: BorderRadius.circular(20),
        ),
        child: InkWell(
          onTap: onPressed,
          child: Center(
            child: Icon(
              Icons.add,
              color: Colors.white,
            ),
          ),
        ),
      ),
    );
  }
}

/************************widget to build total load text and container***************************/
class TotalLoadWidget extends StatelessWidget {
  final double totalLoad;

  TotalLoadWidget({required this.totalLoad});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Text(
          'Total Load',
          style: TextStyle(
            fontSize: 20.0 * MediaQuery.of(context).textScaleFactor,
            fontWeight: FontWeight.bold,
            color: Colors.black,
          ),
        ),
        SizedBox(width: _wid(30)),
        Container(
          height: _hig(40),
          width: _wid(200),
          padding:
              EdgeInsets.symmetric(horizontal: _wid(10), vertical: _hig(10)),
          decoration: BoxDecoration(
            color: Color(0xffD9EDCA),
            borderRadius: BorderRadius.circular(20),
          ),
          child: Center(
            child: Text(
              totalLoad.toStringAsFixed(2),
              style: TextStyle(
                fontSize: 17.0 * MediaQuery.of(context).textScaleFactor,
                fontWeight: FontWeight.bold,
                color: Colors.black,
              ),
            ),
          ),
        ),
      ],
    );
  }
}

/********************widget to build next button**************************/
class NextButtonWidget extends StatelessWidget {
  final VoidCallback onPressed;
  NextButtonWidget({required this.onPressed});
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.only(
        bottom: _hig(12.5),
        top: _hig(12.5),
        right: _wid(12.5),
      ),
      child: Align(
        alignment: Alignment.bottomRight,
        child: SizedBox(
          width: _wid(150),
          height: _hig(40),
          child: ElevatedButton(
            onPressed: onPressed,
            style: ElevatedButton.styleFrom(
              backgroundColor: Color(0xffFB9534),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(30.0),
              ),
            ),
            child: Text(
              'Next >',
              style: TextStyle(
                fontSize: 10.0 * MediaQuery.of(context).textScaleFactor,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
          ),
        ),
      ),
    );
  }
}

/**********************widget for text field container of max load***************************/
class MaxLoadcontainer extends StatefulWidget {
  final String maxload;
  final Function(String) onChanged;
  final String initialValue;
  final TextEditingController controler;

  MaxLoadcontainer(
      {required this.maxload,
      required this.onChanged,
      required this.initialValue,
      required this.controler});

  @override
  State<MaxLoadcontainer> createState() => _MaxLoadcontainerState();
}

class _MaxLoadcontainerState extends State<MaxLoadcontainer> {
  TextEditingController? controller;

  @override
  void initState() {
    controller = widget.controler;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    var w = MediaQuery.of(context).size.width;
    var h = MediaQuery.of(context).size.height;
    var t = MediaQuery.of(context).textScaleFactor;
    return Container(
      // padding: EdgeInsets.only(
      //   left: w * 0.0323,
      //   right: w * 0.0323,
      //   top: 0.01,
      //   bottom: h * 0.01,
      // ),
      width: w * 0.323,
      height: h * 0.051,
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(50),
        color: Color(0xffD9EDCA),
      ),
      child: Center(
        child: TextField(
          
          obscureText: false,
          onChanged: widget.onChanged,
          keyboardType: TextInputType.numberWithOptions(decimal: true),
          inputFormatters: [
            FilteringTextInputFormatter.allow(RegExp(r'^(\d\d*\.|\d)\d*')),
          ],
          textAlign: TextAlign.center,
          controller: controller,
          decoration: InputDecoration(
             contentPadding: EdgeInsets.only(bottom: 0.02 * h),
            border: InputBorder.none,
            hintText: widget.maxload,
            hintStyle: TextStyle(
              color: Color(0xff4A4A4A),
              fontWeight: FontWeight.bold,
              fontSize: 10.0 * t,
            ),
          ),
        ),
      ),
    );
  }
}

class Device {
  String name;
  double waat;
  int numbers;

  Device({
    required this.name,
    required this.waat,
    required this.numbers,
  });
}