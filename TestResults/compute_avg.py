
import os
import subprocess
from collections import OrderedDict

data = {}

def main():
	dirs = [d for d in os.listdir(os.getcwd()) if os.path.isdir(os.path.join(os.getcwd(), d)) and 'plot' not in d]
	
	for d in dirs:
		# print d
		make_avg_file(d)

	# for test in ["notl straight", "notl turns", "wtl straight", "wtl turns"]:
	# 	for attr in ["deadlocks/min", "through/min", "collisions/min", "dead-info-time", "coll-info-time", "coll-info-cars"]:
	# 		for interval in ["20sec", "60sec"]:
	# 			make_graph(attr, test, interval)

	for attr in ["deadlocks/min", "through/min", "collisions/min", "dead-info-time", "coll-info-time", "coll-info-cars"]:
		make_graph_comp(attr)

def make_graph(attr=None, test=None, interval=None):
	global data
	# data = get_avgs()

	if attr is None and test is None and interval is None:
		attr = raw_input("what attribute to display? ")
		test = raw_input("in what test conditions? (notl/wtl straight/turns)  ")
		interval = raw_input("what interval of traffic light conditions? (20sec/60sec)")
	p = subprocess.Popen(['gnuplot', '-'], stdin=subprocess.PIPE)
	cmds = []
	datafn = 'plotdata.txt'
	directory = os.path.join(os.getcwd(),"plots/%s/" % attr.replace('/', ''))
	if 'wtl' in test:
		plotout = os.path.join(directory, 'plot_%s_%s.png' % (test.replace(' ', ''), interval))
	else:
		plotout = os.path.join(directory, 'plot_%s.png' % (test.replace(' ', '')))

	if not os.path.exists(directory):
	    os.makedirs(directory)

	f = open(datafn, 'w+')
	for name,k in data.items():
		if test in name and (('wtl' in test and interval in name) or 'notl' in name):

			# f.write("%d\t%f\n"% (k['carMaxSpeed']+1, k.get(attr)))
			if int(k['carMaxSpeed'])+1 == 30:
				n = 1
			elif int(k['carMaxSpeed'])+1 == 40:
				n = 2
			elif int(k['carMaxSpeed'])+1 == 60:
				n = 3
			else:
				n = 0
			f.write("%d\t%f\n"% (n, k.get(attr)))
	f.close()
	f = p.stdin
	cmds.append('set terminal png')
	cmds.append('set out "%s"'% plotout)
	cmds.append('set xlabel "max speed km/h"')
	if attr == 'deadlocks/min':
		cmds.append('set yrange[0:0.25]')
	elif attr == 'through/min':
		cmds.append('set yrange[0:12.5]')
	elif attr == 'coll-info-time':
		cmds.append('set yrange[0:140]')
	elif attr == 'coll-info-cars':
		cmds.append('set yrange[0:10]')
	elif attr == 'dead-info-time':
		cmds.append('set yrange[0:172]')
	elif attr == 'collisions/min':
		cmds.append('set yrange[0:0.1]')

	cmds.append('set xtics ("30" 1, "40" 2, "60" 3) ')
	cmds.append('set xrange[0:4]')
	cmds.append('set style fill solid 0.5 border -1')
	cmds.append('set boxwidth 0.2')
	cmds.append('set ylabel "%s"'%attr)
	cmds.append('plot "%s" w boxes title "%s"'%(datafn, attr))
	cmds.append('quit')
	print >> f, '\n'.join(cmds)
	f.close()
	p.wait()
	try:
		os.unlink(datafn)
	except:
		pass

def make_graph_comp(attr):
	global data

	# f = open('data disct.txt', 'w+')
	# for name in data:
	# 	f.write('"%s": {'%name)
	# 	# for k,v in data[name].items():
	# 	# 	f.write('"%s": %s\n'%(k,str(v)))
	# 	f.write('}\n')
	# f.write('\n')
	# f.close()
	
	# SORT data dict by max speed
	items = data.items()
	items.sort()
	data = OrderedDict(items)
	# for name in data:
	# 	items = data[name].items()
	# 	items.sort()
	# 	data[name] = OrderedDict(items)

	# # write data to a file for debugging
	# f = open('data disct.txt', 'a')
	# for name in data:
	# 	f.write('"%s": {'%name)
	# 	# for k,v in data[name].items():
	# 	# 	f.write('"%s": %s\n'%(k,str(v)))
	# 	f.write('}\n')
	# f.close()

	p = subprocess.Popen(['gnuplot', '-'], stdin=subprocess.PIPE)
	cmds = []
	# datafn = 'plotdata.txt'
	datafn = 'plotdata_%s.txt'%attr.replace('/', '')
	directory = os.path.join(os.getcwd(),"plots")
	plotout = os.path.join(directory, '%s.png'%attr.replace('/', ''))

	if not os.path.exists(directory):
	    os.makedirs(directory)

	f = open(datafn, 'w+')
	f.write('#%s\n'%attr)

	for test in ["notl straight", "notl turns"]:
		f.write('#%s\n' % test)
		for name,k in data.items():
			if test in name:
				if int(k['carMaxSpeed'])+1 == 30:
					n = 1
				elif int(k['carMaxSpeed'])+1 == 40:
					n = 2
				elif int(k['carMaxSpeed'])+1 == 60:
					n = 3
				else:
					n = 0
				f.write("%d\t%f\n"% (n, k.get(attr)))
		f.write('\n\n')

	for test in ["wtl straight", "wtl turns"]:
			for interval in ["20sec", "60sec"]:
				f.write('#%s %s\n'% (test, interval))
				for name,k in data.items():
					if test in name and interval in name:
						if int(k['carMaxSpeed'])+1 == 30:
							n = 1
						elif int(k['carMaxSpeed'])+1 == 40:
							n = 2
						elif int(k['carMaxSpeed'])+1 == 60:
							n = 3
						else:
							n = 0
						f.write("%d\t%f\n"% (n, k.get(attr)))
				f.write('\n\n')
	f.close()
	f = p.stdin
	cmds.append('set terminal png')
	cmds.append('set out "%s"'% plotout)
	cmds.append('set xlabel "max speed km/h"')
	cmds.append('set xtics ("30" 1, "40" 2, "60" 3) ')
	cmds.append('set xrange[0:4]')
	cmds.append('set style fill solid 0.5 border -1')
	cmds.append('set boxwidth 0.2')
	cmds.append('set ylabel "%s"'%attr)
	cmds.append('set key below')
	cmds.append('plot "%s" index 0 w linespoints title "no t.l. straight",\
	 "%s" index 1 w linespoints title "no t.l. turns",\
	 "%s" index 2 w linespoints lt rgb "blue" title "with t.l. 20sec straight",\
	 "%s" index 3 w linespoints lt rgb "red" title "with t.l. 60sec straight",\
	 "%s" index 4 w linespoints lt rgb "gray" title "with t.l. 20sec turns",\
	 "%s" index 5 w linespoints lt rgb "orange" title "with t.l. 60sec turns"\
	 '%(datafn, datafn, datafn, datafn, datafn, datafn))
	cmds.append('quit')
	print >> f, '\n'.join(cmds)
	f.close()
	p.wait()
	try:
		os.unlink(datafn)
	except:
		pass
	

def clean_data(dictionary):
	for k,v in dictionary.items():
		if v == []:
			dictionary[k] = 0.0


def get_avgs():
	dirs = [d for d in os.listdir(os.getcwd()) if '~' in d and '.txt' in d]
	avgs = {}
	for d in dirs:
		f = open(d, 'r')
		lines = f.readlines()
		f.close()
		dictionary = {}
		for l in lines:
			key = l.split(': ')[0]
			v = l.split(': ')[1].rstrip('\n')
			if '[]' in v:
				v = 0
			elif 'False' in v:
				v = False
			elif 'True' in v:
				v = True
			else:
				v = float(v)
			dictionary[key] = v
		avgs[d] = dictionary
	return avgs


def make_avg_file(d):
	global data
	mydir = os.getcwd()
	files = [ fn for fn in os.listdir(os.path.join(mydir, d)) if 'test' in fn ]
	files.sort()
	info_dicts = []
	for fn in files:
		lines = []
		f = open(os.path.join(mydir, d, fn), 'r')
		lines = f.readlines()
		f.close()
		info = lines[:15]
		hm = [ (s[0], s[-1].rstrip('\n')) for s in [l.split(':') for l in info]]
		extra = ''.join(lines[16:]).split('#')
		# print fn
		# print extra
		extra = extra[1:]
		hm = dict(hm)
		hm['coll-info-cars'] = []
		hm['coll-info-time'] = []
		for i in xrange(int(hm['collisions'])):
			try:
				hm['coll-info-cars'].append(int(extra[0].split('\n')[1+i].split('     ')[0]))
				hm['coll-info-time'].append(float(extra[0].split('\n')[1+i].split('     ')[1]))
			except:
				pass
				# print 'exception parsing coll-info'
		hm['dead-info-cars'] = []
		hm['dead-info-time'] = []
		for i in xrange(int(hm['deadlocks'])):
			try:
				# print extra[1].split('\n')[1+i].split('   ')
				hm['dead-info-cars'].append(int(extra[1].split('\n')[1+i].split('   ')[0]))
				hm['dead-info-time'].append(float(extra[1].split('\n')[1+i].split('   ')[1]))
				# print 'added', extra[1].split('\n')[1+i].split('   ')
				# if 'notl turns' in d and '~40' in d:
				# 	print hm['dead-info-time']
			except:
				pass
				# print 'exception parsing dead-info'
		info_dicts.append(hm)

	#collect everything into one
	avg_dict = info_dicts[0].copy()
	for k,v in avg_dict.items():
		avg_dict[k] = []
	for di in info_dicts:
		for k,v in di.items():
			avg_dict[k].append(v)
	avg_dict['coll-info-cars'] = sum(avg_dict['coll-info-cars'], [])
	avg_dict['coll-info-time'] = sum(avg_dict['coll-info-time'], [])
	avg_dict['dead-info-cars'] = sum(avg_dict['dead-info-cars'], [])
	avg_dict['dead-info-time'] = sum(avg_dict['dead-info-time'], [])

	for k,v in avg_dict.items():
		if not v:
			continue
		if k == 'time':
			avg_dict[k] = reduce(lambda x, y: float(x) + float(y), v)
		elif 'True' not in v and 'False' not in v:
			avg_dict[k] = reduce(lambda x, y: float(x) + float(y), v) / float(len(v))
		else:
			avg_dict[k] = avg_dict[k][0]
	avg_dict['through/min'] = (avg_dict['arrived']/avg_dict['time'])*60
	avg_dict['collisions/min'] = (avg_dict['collisions']/avg_dict['time'])*60
	avg_dict['deadlocks/min'] = (avg_dict['deadlocks']/avg_dict['time'])*60
	# # write to file avgs for this folder
	# f = open(os.path.join(os.getcwd(), d) + '.txt', 'w+')
	# for k,v in avg_dict.items():
	# 	f.write(k + ': ' + str(v) + '\n')
	# f.close()
	clean_data(avg_dict)
	data[d] = avg_dict


if __name__ == '__main__':
	main()