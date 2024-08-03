import 'dart:async';

import 'package:app/MarketPlace/MarketPlace.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';

import '../../main.dart';

class SearchWidgwt extends StatefulWidget {
  final Future<void> Function(String) onpress;
  SearchWidgwt({required this.onpress});

  @override
  State<SearchWidgwt> createState() => _SearchWidgwtState();
}

class _SearchWidgwtState extends State<SearchWidgwt> {
  String v = '';

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: width(590),
      height: height(16),
      child: TextField(
        decoration: InputDecoration(
          hintText: 'Search...',
          filled: true,
          fillColor: Color(0xffD9EDCA),
          suffixIcon: Container(
            width: 5,
            height: 5,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              color: Color(0xffED9555),
            ),
            child: GestureDetector(
                onTap: () {
                  print(v + "**********************");
                  widget.onpress(v);
                },
                child: Icon(
                  Icons.search,
                  size: width(40),
                  color: Colors.white,
                )),
          ),
          border: OutlineInputBorder(
            borderRadius: BorderRadius.circular(20.0),
            borderSide: BorderSide.none,
          ),
          contentPadding:
              EdgeInsets.symmetric(vertical: 0, horizontal: width(10)),
        ),
        style: TextStyle(
          fontSize: 14,
        ),
        textAlign: TextAlign.center,
        onChanged: (value) {
          v = value;

          print('Search query: $value');
        },
      ),
    );
  }
}
