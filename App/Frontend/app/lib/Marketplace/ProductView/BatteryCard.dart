import 'package:app/MarketPlace/APIService/ProductModel.dart';
import 'package:app/MarketPlace/APIService/ProductService.dart';
import 'package:app/main.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

class BatteryCard extends StatefulWidget {
  final BatteryModel batteryModel;

  BatteryCard({required this.batteryModel});

  @override
  State<BatteryCard> createState() => _BatteryCardState();
}

class _BatteryCardState extends State<BatteryCard> {
  //bool isFavorite = false;
  PanelService? panelService = PanelService(Dio());

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: width(370),
      height: height(130),
      child: Card(
        color: Color(0xffD9EDCA), // #D9EDCA
        child: Column(
          children: [
            Align(
              alignment: Alignment.centerLeft,
              child: IconButton(
                onPressed: () async {
                  setState(() {
                    widget.batteryModel.isFavorite =
                        !widget.batteryModel.isFavorite;
                    // add to favorite
                  });
                  print('jnhsCJHD');
                  await panelService?.AddToFavorite(
                      widget.batteryModel.BatteryID);
                  print(widget.batteryModel.BatteryID);
                },
                icon: Icon(
                  widget.batteryModel.isFavorite
                      ? Icons.favorite
                      : Icons.favorite_border,
                  color: widget.batteryModel.isFavorite
                      ? Colors.orange
                      : Colors.black,
                ),
              ),
            ),
            Column(
              children: [
                Align(
                  alignment: Alignment.center,
                  child:
                      _buildImageforBattery(batteryModel: widget.batteryModel),
                ),
                Padding(
                  padding: const EdgeInsets.only(left: 16),
                  child:
                      _TextBuilderForBattery(batteryModel: widget.batteryModel),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

// --------------- ListView for Battery Card ---------------------------
class BatteryCardList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: FutureBuilder<List<BatteryModel>>(
        future: BatteryService(Dio()).getBatteryInfo(),
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text('Error: ${snapshot.error}'));
          } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
            return Center(child: Text('No data available'));
          } else {
            return ListView.builder(
              // (snapshot.data!.length / 2).ceil()
              itemCount: (snapshot.data!.length / 2).ceil(),
              itemBuilder: (context, index) {
                int firstIndex = index * 2;
                int secondIndex = firstIndex + 1;
                bool hasSecond = secondIndex < snapshot.data!.length;
                return Row(
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(left: 8),
                      child:
                          BatteryCard(batteryModel: snapshot.data![firstIndex]),
                    ),
                    if (hasSecond)
                      BatteryCard(batteryModel: snapshot.data![secondIndex]),
                    // Remove duplicate PanelCard widget
                  ],
                );
              },
            );
          }
        },
      ),
    );
  }
}

// ---------------------------------------------------------------
Widget _TextBuilderForBattery({required BatteryModel batteryModel}) {
  return Column(
    crossAxisAlignment: CrossAxisAlignment.start,
    children: [
      Text(
        batteryModel.BatteryBrandName,
        style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold),
      ),
      Row(
        children: [
          Icon(Icons.monetization_on, size: 16),
          Text(
            batteryModel.BatteryPrice,
            style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ],
      ),
      // Row(
      //   children: [
      //     Icon(Icons.location_on, size: 16),
      //     Text(
      //       batteryModel.BatteryPrice ?? 'N/A',
      //       style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
      //     ),
      //   ],
      // ),
      Row(
        children: [
          Icon(Icons.calendar_today, size: 16),
          Text(
            batteryModel.BatteryCapacity,
            style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ],
      ),
    ],
  );
}

SizedBox _buildImageforBattery({required BatteryModel batteryModel}) {
  return SizedBox(
    width: width(220),
    height: height(50),
    // ignore: prefer_interpolation_to_compose_strings
    child: Image.network(
        "http://solareasegp.runasp.net/" + batteryModel.Batteryimage),
  );
}
