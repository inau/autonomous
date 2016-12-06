
import os
import subprocess

data = {}

def main():
	dirs = [d for d in os.listdir(os.getcwd()) if os.path.isdir(os.path.join(os.getcwd(), d))]
	for d in dirs:
		make_avg_file(d)
	for cat in ['wtl straight', 'notl straight', 'wtl turns', 'notl turns']:
		make_graphs('through/min', cat)

def make_graphs(param, category):
	global data
	plotinfo = []
	labels = []
	for k,v in data.items():
		if cat in k:
			plotinfo.append(data[k][param])
			for l in ['fast', 'very slow', 'slow']:
				if l in k:
					labels.append(l)
					break
	return
	p = subprocess.Popen(['gnuplot', '-'], stdin=subprocess.PIPE)
	


def make_avg_file(d):
	global data
	mydir = os.getcwd()
	files = [ fn for fn in os.listdir(os.path.join(mydir, d)) if 'test' in fn ]
	info_dicts = []
	for fn in files:
		f = open(os.path.join(mydir, d, fn), 'r')
		lines = f.readlines()
		f.close()
		info = lines[0:15]
		hm = [ (s[0], s[-1].rstrip('\n')) for s in [l.split(':') for l in info]]
		extra = ''.join(lines[16:-1]).split('#')
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
		hm['dead-info-cars'] = []
		hm['dead-info-time'] = []
		for i in xrange(int(hm['deadlocks'])):
			try:
				hm['dead-info-cars'].append(int(extra[1].split('\n')[1+i].split('   ')[0]))
				hm['dead-info-time'].append(float(extra[1].split('\n')[1+i].split('   ')[1]))
			except:
				pass
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
	#write to file
	f = open(os.path.join(os.getcwd(), d) + '.txt', 'w+')
	for k,v in avg_dict.items():
		f.write(k + ': ' + str(v) + '\n')
	f.close()
	data[d] = avg_dict


if __name__ == '__main__':
	main()