import 'package:app/favorite/APIService/ProductModel.dart';
import 'package:app/favorite/APIService/ProductService.dart';
import 'package:app/favorite/FavoritePage.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

class InverterCard extends StatefulWidget {
  final FavoriteInverter inverterModel;
  final VoidCallback onFavoriteToggled;

  InverterCard({required this.inverterModel, required this.onFavoriteToggled});

  @override
  State<InverterCard> createState() => _InverterCardState();
}

class _InverterCardState extends State<InverterCard> {
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
                            widget.inverterModel.InverterID);
                        setState(() {
                          widget.inverterModel.isFavorite =
                              !widget.inverterModel.isFavorite;
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
                  alignment: Alignment.topCenter,
                  child: _buildImageforInverter(
                      inverterModel: widget.inverterModel),
                ),
                Padding(
                  padding: const EdgeInsets.only(left: 16),
                  child: _TextBuilderForInverter(
                      inverterModel: widget.inverterModel),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

class InverterCardList extends StatefulWidget {
  @override
  _InverterCardListState createState() => _InverterCardListState();
}

class _InverterCardListState extends State<InverterCardList> {
  List<FavoriteInverter> _favoriteInvertersFuture = [];
  bool inverterLoading = false;
  @override
  void initState() {
    super.initState();
    _refreshFavoriteInverters();
  }

  Future<void> _refreshFavoriteInverters() async {
    setState(() {
      inverterLoading = true;
    });
    _favoriteInvertersFuture =
        await InverterService(Dio()).getFavoriteInverter();
    setState(() {
      inverterLoading = false;
    });
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Stack(
        children: [
          ListView.builder(
            itemCount: (_favoriteInvertersFuture.length / 2).ceil(),
            itemBuilder: (context, index) {
              int firstIndex = index * 2;
              int secondIndex = firstIndex + 1;
              bool hasSecond = secondIndex < _favoriteInvertersFuture.length;
              return Padding(
                padding: const EdgeInsets.only(bottom: 8.0),
                child: Row(
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(left: 8.0),
                      child: InverterCard(
                        inverterModel: _favoriteInvertersFuture[firstIndex],
                        onFavoriteToggled: _refreshFavoriteInverters,
                      ),
                    ),
                    if (hasSecond)
                      Padding(
                        padding: const EdgeInsets.only(left: 8.0),
                        child: InverterCard(
                          inverterModel: _favoriteInvertersFuture[secondIndex],
                          onFavoriteToggled: _refreshFavoriteInverters,
                        ),
                      ),
                  ],
                ),
              );
            },
          ),
          if (inverterLoading)
            const Opacity(
                opacity: 0,
                child: ModalBarrier(dismissible: false, color: Colors.black)),
          if (inverterLoading)
            const Center(
              child: CircularProgressIndicator(),
            )
        ],
      ),
    );
  }
}

Widget _TextBuilderForInverter({required inverterModel}) {
  return Column(
    crossAxisAlignment: CrossAxisAlignment.start,
    children: [
      SizedBox(
        height: height(2),
      ),
      Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(
            Icons.branding_watermark,
            size: 16,
          ),
          Flexible(
            child: Text(
              inverterModel.InverterBrandName,
              style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
            ),
          ),
        ],
      ),
      Row(
        children: [
          Icon(
            Icons.price_change,
            size: 16,
          ),
          Text(
            inverterModel.InverterPrice,
            style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ],
      ),
      Row(
        children: [
          Icon(
            Icons.power,
            size: 16,
          ),
          Text(
            inverterModel.InverterCapacity,
            style: TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ],
      ),
    ],
  );
}

SizedBox _buildImageforInverter({required inverterModel}) {
  return SizedBox(
    width: width(220),
    height: height(50),
    child: Image.network(
        "http://solareasegp.runasp.net/" + inverterModel.Inverterimage),
  );
}
