#includes <streamio>
#includes <vector>

std::vector labels;
std::vector values;

char* 	coll_cars, collisions_min, carAcceleration,
		dead_cars, dead_time, canTurn,
		spawnProbability, arrived, present, carMaxSpeed,
		deadlocks, through_min, lost, created, spawnRate, collisions, trafficLightOffset, trafficLightMode,
		time, trafficLightDuration, deadlocks_min, coll_time;

coll-info-cars: 7.0
collisions/min: 0.0163860071364
carAcceleration: 3.0
dead-info-cars: []
dead-info-time: []
canTurn: False
spawnProbability: 20.0
arrived: 184.75
present: 3.25
carMaxSpeed: 59.4
deadlocks: 0.25
through/min: 12.1092592738
lost: 0.0
created: 202.75
spawnRate: 0.764137075
collisions: 0.25
trafficLightOffset: 3.0
trafficLightMode: True
time: 915.4152
trafficLightDuration: 30.0
deadlocks/min: 0.0163860071364
coll-info-time: 108.0055
		
using std::cin;
using std::cout;
using std::endl;

char* header(int columns) {
	std::stringstream buffer;
	buffer << "\begin{tabular}{l";
	for(int i = 0; i < (columns-1); i++) buffer << " c";
	buffer << "}" << std::endl;
	return buffer.str();
}

void main(int argc, char **argv) {
	char *lbl, *val;
	while(lbl << cin) {
		val << cin;
		labels.push_back(lbl);
		values.push_back(val);
	}
	
	cout << header( labels.length );
	for(int i = 0; i < labels.length) {
		
	}
	cout << "\end{tabular}" << endl;
	
	
	
}