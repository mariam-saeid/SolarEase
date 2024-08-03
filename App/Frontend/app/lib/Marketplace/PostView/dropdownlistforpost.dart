import 'package:app/MarketPlace/ProductView/Search.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../../main.dart';

class DownListPost extends StatefulWidget {
  final Function(String) onCategoryChanged;

  DownListPost({required this.onCategoryChanged});

  @override
  State<DownListPost> createState() => _DownListPostState();
}

class _DownListPostState extends State<DownListPost> {
  String _selectedItem = 'all';

  @override
  Widget build(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Container(
          padding: EdgeInsets.only(left: 10),
          height: height(20),
          width: width(220),
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(12),
          ),
          child: DropdownButton<String>(
            focusColor: Color(0xffFAFAFA),
            dropdownColor: Color(0xffFAFAFA),
            elevation: 0,
            icon: Icon(Icons.arrow_drop_down, color: Colors.orange),
            underline: Container(),
            style: TextStyle(color: Colors.black, fontWeight: FontWeight.bold),
            value: _selectedItem,
            items: <String>['all', 'Panel', 'Inverter', 'Battery', 'other']
                .map((String value) {
              return DropdownMenuItem<String>(
                value: value,
                child: Align(
                  alignment: Alignment.center,
                  child: Text(
                    value,
                    textAlign: TextAlign.center,
                    style: TextStyle(
                      fontSize: 15.0 * t,
                      fontWeight: FontWeight.bold,
                      color: Color(0xff063221),
                    ),
                  ),
                ),
              );
            }).toList(),
            onChanged: (newValue) {
              setState(() {
                _selectedItem = newValue!;
                widget.onCategoryChanged(_selectedItem);
              });
            },
          ),
        ),
        Container(
            width: width(250), height: height(3), color: Color(0xffBBBAB6)),
        SizedBox(height: 8),
      ],
    );
  }
}
