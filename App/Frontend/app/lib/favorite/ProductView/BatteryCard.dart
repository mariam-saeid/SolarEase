import 'package:app/favorite/APIService/ProductModel.dart';
import 'package:app/favorite/APIService/ProductService.dart';
import 'package:app/favorite/FavoritePage.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

class BatteryCard extends StatefulWidget {
  final FavoriteBattery batteryModel;
  final VoidCallback onFavoriteToggled;

  BatteryCard({required this.batteryModel, required this.onFavoriteToggled});

  @override
  State<BatteryCard> createState() => _BatteryCardState();
}

class _BatteryCardState extends State<BatteryCard> {
  PanelService? panelService = PanelService(Dio());
  bool _isProcessing = false;

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
              child: _isProcessing
                  ? CircularProgressIndicator()
                  : IconButton(
                      onPressed: () async {
                        setState(() {
                          _isProcessing = true;
                        });
                        await panelService?.ToggleFavorite(
                            widget.batteryModel.BatteryID);
                        setState(() {
                          widget.batteryModel.isFavorite =
                              !widget.batteryModel.isFavorite;
                          _isProcessing = false;
                        });
                        widget.onFavoriteToggled();
                      },
                      icon: Icon(
                        Icons.favorite,
                        color: Colors.orange,
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

class BatteryCardList extends StatefulWidget {
  @override
  _BatteryCardListState createState() => _BatteryCardListState();
}

class _BatteryCardListState extends State<BatteryCardList> {
  List<FavoriteBattery> _favoriteBatteriesFuture = [];
  bool batteryLoading = false;

  @override
  void initState() {
    super.initState();
    _refreshFavoriteBatteries();
  }

  Future<void> _refreshFavoriteBatteries() async {
    setState(() {
      batteryLoading = true;
    });
    _favoriteBatteriesFuture = await BatteryService(Dio()).getFavoriteBattery();
    setState(() {
      batteryLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Stack(
        children: [
          ListView.builder(
            itemCount: (_favoriteBatteriesFuture.length / 2).ceil(),
            itemBuilder: (context, index) {
              int firstIndex = index * 2;
              int secondIndex = firstIndex + 1;
              bool hasSecond = secondIndex < _favoriteBatteriesFuture.length;
              return Padding(
                padding: const EdgeInsets.only(bottom: 8.0),
                child: Row(
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(left: 8.0),
                      child: BatteryCard(
                        batteryModel: _favoriteBatteriesFuture[firstIndex],
                        onFavoriteToggled: _refreshFavoriteBatteries,
                      ),
                    ),
                    if (hasSecond)
                      Padding(
                        padding: const EdgeInsets.only(left: 8.0),
                        child: BatteryCard(
                          batteryModel: _favoriteBatteriesFuture[secondIndex],
                          onFavoriteToggled: _refreshFavoriteBatteries,
                        ),
                      ),
                  ],
                ),
              );
            },
          ),
          if (batteryLoading)
            const Opacity(
                opacity: 0,
                child: ModalBarrier(dismissible: false, color: Colors.black)),
          if (batteryLoading)
            const Center(
              child: CircularProgressIndicator(),
            )
        ],
      ),
    );
  }
}

Widget _TextBuilderForBattery({required batteryModel}) {
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

SizedBox _buildImageforBattery({required batteryModel}) {
  return SizedBox(
    width: width(220),
    height: height(50),
    child: Image.network(
        "http://solareasegp.runasp.net/" + batteryModel.Batteryimage),
  );
}
