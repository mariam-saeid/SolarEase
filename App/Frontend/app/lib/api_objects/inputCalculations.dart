class Calculationsdata {
  bool ongrid = false;
  String maxOrbills = "bills";
  ElectricalBills electricalBills = ElectricalBills();
  double maxusageon = -1;
  double maxusageoff = -1;
  double elecitycoverage = -1;
  double maxload = -1;

}

class ElectricalBills {
  Map<String, num> months = {
    'January': -1,
    "February": -1,
    "March": -1,
    "April": -1,
    "May": -1,
    "June": -1,
    "July": -1,
    "August": -1,
    "September": -1,
    "October": -1,
    "November": -1,
    "December": -1
  };
  bool isEmpty() {
    for (var value in months.values) {
      if (value < 0) return true;
    }
    return false;
  }

  bool ok() {
    int numofBad = 0;
    for (var value in months.values) {
      if (value <= 0) return false;
    }

    return true;
  }
}
