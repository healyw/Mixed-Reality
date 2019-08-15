# Mixed-Reality
Condition Monitoring Tech Display
## Authorization
Project started by Will Healy and Kyle Miller. Given to Samuel Ray and Paul Belyea for the Condition Monitoring Demo. This project began on May 15th, 2019 and was completed on August 16th, 2019.
## Project Intention
This project is intended to show what life could be like with industrial manufacturing in AR. It currently displays 7 different Balluff Products:

Product | Description
-------- | ----------
BSP 008T | Pressure Sensor
BNI 0082 | Smartlight
BIP 001F | Inductive Sensor
BOS 026R | ADCAP RFID Sensor
BTL 1F5N | Linear Transducer
BNI 005H | Masterblock
BAE 00TK | Power Supply

## Useful Code Links
### ![HTML Updater](https://github.com/healyw/Mixed-Reality/blob/master/documentation-code/HtmlUpdater.cs)
A C# Script for import into Unity. 
- Add your url and type of product, data will be fetched from that url and search for specific PLC formatted datatypes. 
- PLC format: '<td>Sensor Status: :="DataBlock1"."MyData":</td>'.
### ![JSON Updater](https://github.com/healyw/Mixed-Reality/blob/master/documentation-code/JsonUpdater.cs)
A C# Script for import into Unity (Drag under your scripts in Hierarchy).
- Add url, type of product, and what you want to get from the masterblock JSON list.
