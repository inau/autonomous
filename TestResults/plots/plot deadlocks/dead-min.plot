#set term png
#set out "plot_deadlocksmin_notl.png"
#set yrange [0:0.25]
#set xrange [0:4]
#set xtics( "30" 1, "40" 2, "60" 3)
#set key below
#set xlabel "car maximum speed [km/h]"
#set ylabel "deadlocks/min"
#set title "Deadlocks without traffic lights"
#file = "plotdata_deadlocksmin_notl.txt"
#plot file index 0 w linespoints title "only straight",\
#	 file index 1 w linespoints title "with turns"
#	 
#
#set term png
#set out "plot_deadlocksmin_tl.png"
#set yrange [0:0.25]
#set xrange [0:4]
#set xtics( "30" 1, "40" 2, "60" 3)
#set key below
#set xlabel "car maximum speed [km/h]"
#set ylabel "deadlocks/min"
#set title "Deadlocks with traffic lights (1 minute interval)"
#file = "plotdata_deadlocksmin_tl.txt"
#plot file index 0 w linespoints lt rgb "red" title "straight",\
#	 file index 1 w linespoints lt rgb "orange" title "with turns"
#
#
#set term png
#set out "plot_deadlocksmin_tl20.png"
#set yrange [0:0.25]
#set xrange [0:4]
#set xtics( "30" 1, "40" 2, "60" 3)
#set key below
#set xlabel "car maximum speed [km/h]"
#set ylabel "deadlocks/min"
#set title "Deadlocks with traffic lights (20 seconds interval)"
#file = "plotdata_deadlocksmin_tl20.txt"
#plot file index 0 w linespoints lt rgb "blue" title "straight",\
#	 file index 1 w linespoints lt rgb "gray" title "with turns"


set term png
set out "plot_deadlocksstraight.png"
set yrange [0:0.25]
set xrange [0:4]
set xtics( "30" 1, "40" 2, "60" 3)
set key below
set xlabel "car maximum speed [km/h]"
set ylabel "deadlocks/min"
set title "Deadlocks cars going straight"
fileno = "plotdata_deadlocksmin_notl.txt"
filetl = "plotdata_deadlocksmin_tl.txt"
file20 = "plotdata_deadlocksmin_tl20.txt"
plot fileno index 0 w linespoints lt rgb "green" title "pure reactive",\
	filetl index 0 w linespoints lt rgb "red" title "traffic lights 60 sec",\
	file20 index 0 w linespoints lt rgb "blue" title "traffic lights 20 sec"

set term png
set out "plot_deadlocksturns.png"
set yrange [0:0.25]
set xrange [0:4]
set xtics( "30" 1, "40" 2, "60" 3)
set key below
set xlabel "car maximum speed [km/h]"
set ylabel "deadlocks/min"
set title "Deadlocks cars making turns"
fileno = "plotdata_deadlocksmin_notl.txt"
filetl = "plotdata_deadlocksmin_tl.txt"
file20 = "plotdata_deadlocksmin_tl20.txt"
plot fileno index 1 w linespoints lt rgb "green" title "pure reactive",\
	filetl index 1 w linespoints lt rgb "red" title "traffic lights 60 sec",\
	file20 index 1 w linespoints lt rgb "blue" title "traffic lights 20 sec"
	 
