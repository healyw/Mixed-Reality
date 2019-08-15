# Mixed-Reality
Condition Monitoring Tech Display started by Will Healy and given to Samuel Ray and Paul Belyea
- Documentation by Samuel Ray from Summer 2019 Semester
- Counter Built by Paul Belyea and Samuel Ray
## Authorization
Project started by Will Healy and given to Samuel Ray and Paul Belyea for the Condition Monitoring Demo. This project began on May 15th, 2019 and was completed on August 16th, 2019.
## Project Intention
This project is intended to show what life could be like with industrial manufacturing in AR. It currently displays 6 different Balluff Products:
- BSP 008T: Pressure Sensor
- BNI 0082: Smartlight
- BIP 001F: Inductive Sensor
- BOS 026R: ADCAP RFID Sensor
- BTL 1F5N: Linear Transducer
- BNI 005H: Masterblock
- BAE 00TK: Power Supply
## Useful Code Links
### ![HTML Updater](https://github.com/healyw/Mixed-Reality/blob/master/documentation-code/HtmlUpdater.cs)
Add your url and type of product, data will be fetched from that url and search for specific PLC formatted datatypes. 
- PLC format: <td>Sensor Status: :="DataBlock1"."MyData":</td>
### ![JSON Updater](https://github.com/healyw/Mixed-Reality/blob/master/documentation-code/JsonUpdater.cs)
Add url, type of product, and what you want to get from the masterblock JSON list.
