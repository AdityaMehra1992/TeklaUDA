import os               # for the environment variable necessary, this is a great tool
import platform         # checks the python platform
import pandas as pd
import string
from sofistik_daten import *
from ctypes import *    # read the functions from the cdb
import math
import plotly.express as px
import plotly.graph_objects as go
from plotly.subplots import make_subplots

######################

path = os.environ["Path"]

# 64bit DLLs
dllPath = r"C:\Program Files\SOFiSTiK\2022\SOFiSTiK 2022\interfaces\64bit"
dllPath += ";"

    # other necessary DLLs
dllPath += r"C:\Program Files\SOFiSTiK\2022\SOFiSTiK 2022"
os.environ["Path"] = dllPath + ";" + path

# Get the DLL functions
myDLL = cdll.LoadLibrary("sof_cdb_w_edu-2022.dll")
py_sof_cdb_get = cdll.LoadLibrary("sof_cdb_w_edu-2022.dll").sof_cdb_get
py_sof_cdb_get.restype = c_int

py_sof_cdb_kenq = cdll.LoadLibrary("sof_cdb_w_edu-2022.dll").sof_cdb_kenq_ex 
#End Region
############################


def GetNodesPosition(Cdb_Link):
    Index = c_int()
    cdbIndex = 99
    
    fileName = Cdb_Link
    Index.value = myDLL.sof_cdb_init(fileName.encode('utf-8'), cdbIndex)
    
    cdbStat = c_int()
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    
    pos = c_int(0)
    datalen = c_int(0)

    a = c_int()
    ie = c_int(0)
    
    datalen.value = sizeof(CNODE)
    RecLen = c_int(sizeof(cnode))
    
    NodeNum = []
    IntNodeNum = []
    X_Coor = []
    Y_Coor = []
    Z_Coor = []
    
    
    while ie.value < 2:
        ie.value = py_sof_cdb_get(Index, 20, 0, byref(cnode), byref(RecLen), 1)
        NodeNum.append(cnode.m_nr)
        IntNodeNum.append(cnode.m_inr)
        X_Coor.append(round(cnode.m_xyz[0],3))  # x coordinates
        Y_Coor.append(round(cnode.m_xyz[1],3))  # y coordinates
        Z_Coor.append(round(cnode.m_xyz[2],3))  # z coordinates
        
    Data = {'NodeNum': NodeNum, 'IntNodeNum' : IntNodeNum, 'X_Coor': X_Coor, 'Y_Coor': Y_Coor, 'Z_Coor': Z_Coor
            }
        
    
    myDLL.sof_cdb_close(0)
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    #if cdbStat.value == 0:
    #print("CDB closed successfully, status = 0")
    
    return pd.DataFrame(Data)

def GetBeamData(Cdb_Link):


    Index = c_int()
    cdbIndex = 99
    
    fileName = Cdb_Link
    Index.value = myDLL.sof_cdb_init(fileName.encode('utf-8'), cdbIndex)
    
    cdbStat = c_int()
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    
    pos = c_int(0)
    datalen = c_int(0)

    a = c_int()
    ie = c_int(0)
    
    datalen.value = sizeof(CBEAM)
    RecLen = c_int(sizeof(cbeam))
    
    BeamNum = []
    Node1 = []
    Node2 = []
    

    
    
    while ie.value < 2:
        ie.value = py_sof_cdb_get(Index, 100, 0, byref(cbeam), byref(RecLen), 1)
        BeamNum.append(cbeam.m_nr)
        Node1.append(cbeam.m_node[0])
        Node2.append(cbeam.m_node[1])
       
        
    Data = {'BeamNum': BeamNum , 'Node1' : Node1 , 'Node2' : Node2} 
        
    
    myDLL.sof_cdb_close(0)
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    #if cdbStat.value == 0:
    #print("CDB closed successfully, status = 0")
    
    return pd.DataFrame(Data)

def GetBeamNodesPosition(GroupNumberList,BeamNodesData, NodePos):

    NodeMultiplier =10000
    X_Coor_1 =[]
    Y_Coor_1 = []
    Z_Coor_1 = []
    X_Coor_2 =[]
    Y_Coor_2 = []
    Z_Coor_2 = []
 
    DataFrame =  pd.DataFrame({'BeamNum': [] , 'Node1' : [] , 'Node2' : []})
    
    for x in GroupNumberList:
        DomainStart = x*NodeMultiplier
        DomainEnd  = DomainStart + NodeMultiplier
        DF = BeamNodesData[(BeamNodesData['BeamNum'] >= DomainStart ) & (BeamNodesData['BeamNum'] <= DomainEnd )]
        DataFrame = pd.concat([DataFrame, DF], axis=0)
    
    N1 =DataFrame['Node1'].tolist()
    N2 =DataFrame['Node2'].tolist()
    NodesTuple = list(zip(N1, N2))
    
    for x in NodesTuple:
        X_Coor_1.append(NodePos[NodePos['NodeNum'] == x[0]]['X_Coor'].tolist()[0])    
        Y_Coor_1.append(NodePos[NodePos['NodeNum'] == x[0]]['Y_Coor'].tolist()[0])
        Z_Coor_1.append(NodePos[NodePos['NodeNum'] == x[0]]['Z_Coor'].tolist()[0])

        X_Coor_2.append(NodePos[NodePos['NodeNum'] == x[1]]['X_Coor'].tolist()[0])
        Y_Coor_2.append(NodePos[NodePos['NodeNum'] == x[1]]['Y_Coor'].tolist()[0])
        Z_Coor_2.append(NodePos[NodePos['NodeNum'] == x[1]]['Z_Coor'].tolist()[0])
    
    BeamNumberlist =  list(map(lambda x: int(x),DataFrame['BeamNum'].tolist()))
    Node1List = list(map(lambda x: int(x),DataFrame['Node1'].tolist()))
    Node2List = list(map(lambda x: int(x),DataFrame['Node2'].tolist()))
    
    MDF= pd.DataFrame({'BeamNum': BeamNumberlist,'Node1': Node1List, 'Node2': Node2List, 'X_Coor_1': X_Coor_1, 'Y_Coor_1': Y_Coor_1 , 'Z_Coor_1': Z_Coor_1, 'X_Coor_2': X_Coor_2, 'Y_Coor_2': Y_Coor_2, 'Z_Coor_2': Z_Coor_2})
    MDF=MDF.sort_values(by=['BeamNum'], ascending=True)
    return MDF

def GetContiniousSegments(DataFrame):
    TraceNum =1
    Trace =[1]
    for i in range(0,len(DataFrame.index)-1):
        if(DataFrame['Node2'][DataFrame.index[i]] == DataFrame['Node1'][DataFrame.index[i+1]]):
            Trace.append(TraceNum)
        else:
            TraceNum = TraceNum+1
            Trace.append(TraceNum)
    try:
      DataFrame.insert(3, "Trace", Trace, False)
    except:
      DataFrame = DataFrame.drop('Trace', axis=1)
      DataFrame.insert(3, "Trace", Trace, False)
    return DataFrame

##### Force Extraction and DF 
def GetBeamForce(Cdb_Link, LC):

    Index = c_int()
    cdbIndex = 99
    
    fileName = Cdb_Link
    Index.value = myDLL.sof_cdb_init(fileName.encode('utf-8'), cdbIndex)
    
    cdbStat = c_int()
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    
    pos = c_int(0)
    datalen = c_int(0)

    a = c_int()
    ie = c_int(0)
    
    datalen.value = sizeof(CBEAM_FOR)
    RecLen = c_int(sizeof(cbeam_for))
    
    BeamNum = []
    Len = []
    NF = []
    Vy = []
    Vz = []
    T = []
    My = []
    Mz = []
    

    
    while ie.value < 2:
        ie.value = py_sof_cdb_get(Index, 102, LC, byref(cbeam_for), byref(RecLen), 1)
        BeamNum.append(cbeam_for.m_nr)
        Len.append(cbeam_for.m_x)
        NF.append(round(cbeam_for.m_n,3))
        Vy.append(round(cbeam_for.m_vy,3))
        Vz.append(round(cbeam_for.m_vz,3))
        T.append(round(cbeam_for.m_mt,3))
        My.append(round(cbeam_for.m_my,3))
        Mz.append(round(cbeam_for.m_mz,3))
        
    Data = {'BeamNum': BeamNum , 'Len' : Len ,'LC': [int(LC)]*len(NF) ,'NF' : NF , 'VY': Vy, 'VZ': Vz, 'T': T, 'My': My, 'Mz': Mz  } 
        
    
    myDLL.sof_cdb_close(0)
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    
    return pd.DataFrame(Data)

def GetBeamEigenForce(Cdb_Link, LC):

    Index = c_int()
    cdbIndex = 99
    
    fileName = Cdb_Link
    Index.value = myDLL.sof_cdb_init(fileName.encode('utf-8'), cdbIndex)
    
    cdbStat = c_int()
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    
    pos = c_int(0)
    datalen = c_int(0)

    a = c_int()
    ie = c_int(0)
    
    datalen.value = sizeof(CBEAM_CRF)
    RecLen = c_int(sizeof(cbeam_crf))
    
    BeamNum = []
    Len = []
    NF = []
    Vy = []
    Vz = []
    T = []
    My = []
    Mz = []
    
    

    
    while ie.value < 2:
        ie.value = py_sof_cdb_get(Index, 104, LC, byref(cbeam_crf), byref(RecLen), 1)
        BeamNum.append(cbeam_crf.m_nr)
        Len.append(cbeam_crf.m_x)
        NF.append(round(cbeam_crf.m_sdni,3))
        Vy.append(round(cbeam_crf.m_sdvy,3))
        Vz.append(round(cbeam_crf.m_sdvz,3))
        T.append(round(cbeam_crf.m_sdmt,3))
        My.append(round(cbeam_crf.m_sdmy,3))
        Mz.append(round(cbeam_crf.m_sdmz,3))

        
    Data = {'BeamNum': BeamNum , 'Len' : Len  ,'LC': [int(LC)]*len(NF)  ,'NF' : NF , 'VY': Vy, 'VZ': Vz, 'T': T, 'My': My, 'Mz': Mz  } 
        
    
    myDLL.sof_cdb_close(0)
    cdbStat.value = myDLL.sof_cdb_status(Index.value)
    
    return pd.DataFrame(Data)

def ForcesForDataFrame(ForceDataFrame, BeamDataFrame):
    BeamNumList = BeamDataFrame['BeamNum']
    SegLen = []
    Chainage = []
    NF=[]
    VY=[]
    VZ=[]
    T=[]
    MY=[]
    MZ=[]
    LC = ForceDataFrame['LC'][0]
    ForceDF = ForceDataFrame[ForceDataFrame['BeamNum'].isin(BeamNumList)]
    
    for i in BeamNumList:
        df = ForceDF[ForceDF['BeamNum']==i].sort_values(by=['Len'], ascending=True)
        Chainage.append(sum(SegLen))
        SegLen.append(df['Len'][df.index[1]])
        NF.append((df['NF'][df.index[0]],df['NF'][df.index[1]]))
        VY.append((df['VY'][df.index[0]],df['VY'][df.index[1]]))
        VZ.append((df['VZ'][df.index[0]],df['VZ'][df.index[1]]))
        T.append((df['T'][df.index[0]],df['T'][df.index[1]]))
        MY.append((df['My'][df.index[0]],df['My'][df.index[1]]))
        MZ.append((df['Mz'][df.index[0]],df['Mz'][df.index[1]]))
    
    BeamNumberlist =  list(map(lambda x: int(x),BeamNumList))
    Node1List = list(map(lambda x: int(x),BeamDataFrame['Node1'].tolist()))
    Node2List = list(map(lambda x: int(x),BeamDataFrame['Node2'].tolist()))
    
    Data = {'BeamNum': BeamNumberlist,'Node1': Node1List, 'Node2': Node2List ,'Chainage': Chainage, 'SegLen':SegLen,'LC': [LC] * len(BeamNumberlist), 'NF': NF, 'VY' : VY, 'VZ': VZ, 'T': T, 'My': MY , 'Mz': MZ}
    
    
    
    return pd.DataFrame(Data)
   
def EigenForcesForDataFrame(ForceDataFrame, BeamDataFrame):

    BeamNumList = BeamDataFrame['BeamNum']
    SegLen = []
    Chainage = []
    NF=[]
    VY=[]
    VZ=[]
    T=[]
    MY=[]
    MZ=[]
    LC = ForceDataFrame['LC'][0]
    ForceDF = ForceDataFrame[ForceDataFrame['BeamNum'].isin(BeamNumList)]
    
    for i in BeamNumList:
        df = ForceDF[ForceDF['BeamNum']==i]
        df_0index = df[df['Len'] ==0].index[0]
        df_1index = df[df['Len'] >0].index[0]
        
        L1 = df.loc[df_0index]
        L2 = df.loc[df_1index]
        
        Chainage.append(sum(SegLen))
        SegLen.append(L2[1])
        NF.append((L1[3],L2[3]))
        VY.append((L1[4],L2[4]))
        VZ.append((L1[5],L2[5]))
        T.append((L1[6],L2[6]))
        MY.append((L1[7],L2[7]))
        MZ.append((L1[8],L2[8]))
    
    BeamNumberlist =  list(map(lambda x: int(x),BeamNumList))
    Node1List = list(map(lambda x: int(x),BeamDataFrame['Node1'].tolist()))
    Node2List = list(map(lambda x: int(x),BeamDataFrame['Node2'].tolist()))
    
    Data = {'BeamNum': BeamNumberlist,'Node1': Node1List, 'Node2': Node2List ,'Chainage': Chainage, 'SegLen':SegLen,'LC': [LC] * len(BeamNumberlist), 'NF': NF, 'VY' : VY, 'VZ': VZ, 'T': T, 'My': MY , 'Mz': MZ}
    
    
    
    return pd.DataFrame(Data)


######Plotting 
def Plot3d(DataFrameTraces, Name):
    fig = go.Figure()
    TraceList = list(set(DataFrameTraces['Trace']))
    Col = px.colors.qualitative.Alphabet + px.colors.qualitative.Alphabet
    ColInd = 0
    Xmax,Xmin, Ymax,Ymin, Zmax, Zmin = -500000,500000,-500000,500000,-500000,500000

    for Trace in TraceList:
        C = DataFrameTraces[DataFrameTraces['Trace'] ==Trace]
        Group = str(int(C['BeamNum'].tolist()[0]/10000))
        BeamNum = C['BeamNum'].tolist()
        BeamNumBef= [0] + BeamNum
        BeamNumAff= BeamNum + [0]
        TraceL = [Trace]*(len(BeamNum)+1)
        Nodes = C['Node1'].tolist() + [C['Node2'].tolist()[-1]]
        X1 = C['X_Coor_1'].tolist() + [C['X_Coor_2'].tolist()[-1]]
        Y1 = C['Y_Coor_1'].tolist() + [C['Y_Coor_2'].tolist()[-1]]
        Z1 = C['Z_Coor_1'].tolist() + [C['Z_Coor_2'].tolist()[-1]]

        Text = list (map(lambda x,y,z,BB,BA,NodeNum,Trace: "Coor: "+str(x)+","+str(y)+","+str(z)+"<br>"+"EleNo."+str(int(BB))+" / "+str(int(BA))+"<br>"+"Node:"+str(int(NodeNum))+"<br>"+"Trace: "+str(Trace),X1,Y1,Z1,BeamNumBef,BeamNumAff,Nodes,TraceL))


        fig.add_trace(go.Scatter3d(x=X1, y=Y1, z = Z1, text = Text,
                            mode='lines', 
                            hoverinfo='text',
                            name= Group+'_'+str(Trace),
                            line=dict(
                            color=Col[ColInd],
                            width=8 
                            )
                            ))
        ColInd = ColInd+1

        if Xmax < max(X1):
            Xmax= max(X1)
        if Ymax < max(Y1):
            Ymax= max(Y1)
        if Zmax < max(Z1):
            Zmax= max(Z1)

        if Xmin > min(X1):
            Xmin= min(X1)
        if Ymin > min(Y1):
            Ymin= min(Y1)
        if Zmin > min(Z1):
            Zmin= min(Z1)

    Range= max(Xmax-Xmin, Ymax-Ymin, Zmax-Zmin)
    RX = (Xmin-20, Xmin+Range+20)
    RY = (Ymin-20, Ymin+Range+20)
    RZ = (Zmin-20, Zmin+Range+20)

    fig.update_layout( width =1200, height = 1200, template="plotly_white", title_text = Name)
    fig.update_scenes(
            xaxis = dict(nticks=4, range=[RX[0],RX[1]],),
            yaxis = dict(nticks=4, range=[RY[0],RY[1]],),
            zaxis = dict(nticks=4, range=[RZ[0],RZ[1]])
        )
    fig.update_layout(scene_aspectmode='cube')
    return fig
        
def Plot2dbeam (ForceDataFrame, ListofPlots, Title):

    RowPlots = 3

    dic = {'NF': "Axial Force[kN]", 'VY' : "Shear Force (Vy) [kN]",'VZ' : "Shear Force (Vz) [kN]", 'T' : "Torsion (T) [kN-m]", 'My': "Moment(MY)[kN-m]", 'Mz': "Moment(MZ)[kN-m]" }
    
    LoadCase = str(ForceDataFrame['LC'][0])
    
    fig = make_subplots(
            rows=math.ceil(len(ListofPlots)/RowPlots), cols=RowPlots,
            subplot_titles=list(map(lambda name: dic[name],ListofPlots ))
            )
    
    #chainage values
    BeamNum = ForceDataFrame['BeamNum'].tolist()
    BeamNumBef= [0,0] 
    BeamNumAff= [0]
    
    Chainage = [0]
    dfc = ForceDataFrame['Chainage']
    sl = ForceDataFrame['SegLen']
    for i in range(0,len(dfc)):
        Chainage.append(dfc[i])
        Chainage.append(dfc[i]+sl[i])
        BeamNumBef.append(BeamNum[i])
        BeamNumBef.append(BeamNum[i])
        BeamNumAff.append(BeamNum[i])
        BeamNumAff.append(BeamNum[i])
        
        
    Chainage.append(Chainage[-1])
    BeamNumBef.append(0)
    BeamNumAff.append(0)
    BeamNumAff.append(0)
    
    RangeMax = max(Chainage)
    RangeMin = min(Chainage)
    RMax =  RangeMax+ abs(RangeMax-RangeMin)*0.1
    RMin =  RangeMin- abs(RangeMax-RangeMin)*0.1
    
    Text =  list (map(lambda Chain, BB,BA: "Chainage: "+ str(round(Chain,2)) +'<br>' +"EleNo.: " + str(BB)+" / "+ str(BA), Chainage, BeamNumBef, BeamNumAff))
    
    ## values and add trace
    for j in range(0,len(ListofPlots)):
        Val = [0]
        nf = ForceDataFrame[ListofPlots[j]]
        for k in nf:
            Val.append(k[0])
            Val.append(k[1])
        Val.append(0)
        
        Valmax = max(Val)
        Valmin = min(Val)
        maxInd = Val.index(Valmax)
        minInd = Val.index(Valmin)

        R = math.ceil((j+1)/RowPlots)
        C = (j+1)-(R -1)*RowPlots
        
        fig.add_trace(go.Scatter(x=Chainage, y=Val, text=Val, hoverinfo='text'),
                     row = R ,col = C )
        fig.add_trace(
                       go.Scatter(x=Chainage, y=[0]* len(Val), text=Text, hoverinfo='text', line = dict(color = 'black', width =.5, dash ='dash'))
                      ,row = R ,col = C )
        
        fig.add_trace(
                       go.Scatter(x=[Chainage[maxInd], Chainage[minInd]], y=[Valmax,Valmin], mode="markers+text",  text=[round(Valmax,1),round(Valmin,1)],textposition=["bottom right","top right"]),
                       row = R ,col = C
                        )     
        
      
        fig.update_xaxes(title_text='Chainage[m]', range= [RMin,RMax])
        fig.update_layout( template="plotly_white")                
        
    fig.update_layout(height=600, width=1800,showlegend=False,
                  title_text= Title+": LC -"+LoadCase)     

    return fig

def Plot2dVertical (ForceDataFrame, ListofPlots, Title, NegativeY):
    if len(ListofPlots)>3:
        RowPlots = len(ListofPlots)
    else :
        RowPlots = 3
        
    dic = {'NF': "Axial Force[kN]", 'VY' : "Shear Force (Vy) [kN]",'VZ' : "Shear Force (Vz) [kN]", 'T' : "Torsion (T) [kN-m]", 'My': "Moment(MY)[kN-m]", 'Mz': "Moment(MZ)[kN-m]" }
    
    LoadCase = str(ForceDataFrame['LC'][0])
    
    fig = make_subplots(
            rows=math.ceil(len(ListofPlots)/RowPlots), cols=RowPlots,
            subplot_titles=list(map(lambda name: dic[name],ListofPlots ))
            )
    
    #chainage values
    BeamNum = ForceDataFrame['BeamNum'].tolist()
    BeamNumBef= [0,0] 
    BeamNumAff= [0]
    
    Chainage = [0]
    dfc = ForceDataFrame['Chainage']
    sl = ForceDataFrame['SegLen']
    for i in range(0,len(dfc)):
        Chainage.append(dfc[i])
        Chainage.append(dfc[i]+sl[i])
        BeamNumBef.append(BeamNum[i])
        BeamNumBef.append(BeamNum[i])
        BeamNumAff.append(BeamNum[i])
        BeamNumAff.append(BeamNum[i])
        
        
    Chainage.append(Chainage[-1])
    BeamNumBef.append(0)
    BeamNumAff.append(0)
    BeamNumAff.append(0)
    
    if (NegativeY == True):
        Chainage1 = list(map(lambda x: -x, Chainage))
        Chainage = Chainage1
    
    RangeMax = max(Chainage)
    RangeMin = min(Chainage)
    RMax =  RangeMax+ abs(RangeMax-RangeMin)*0.1
    RMin =  RangeMin- abs(RangeMax-RangeMin)*0.1
    
    Text =  list (map(lambda Chain, BB,BA: "Chainage: "+ str(round(Chain,2)) +'<br>' +"EleNo.: " + str(BB)+" / "+ str(BA), Chainage, BeamNumBef, BeamNumAff))
    
    ## values and add trace
    for j in range(0,len(ListofPlots)):
        Val = [0]
        nf = ForceDataFrame[ListofPlots[j]]
        for k in nf:
            Val.append(k[0])
            Val.append(k[1])
        Val.append(0)
        
        Valmax = max(Val)
        Valmin = min(Val)
        maxInd = Val.index(Valmax)
        minInd = Val.index(Valmin)

        R = math.ceil((j+1)/RowPlots)
        C = (j+1)-(R -1)*RowPlots
        
        fig.add_trace(go.Scatter(x=Val, y=Chainage, text=Val, hoverinfo='text'),
                     row = R ,col = C )
        fig.add_trace(
                       go.Scatter(x=[0]* len(Val), y=Chainage, text=Text, hoverinfo='text', line = dict(color = 'black', width =.5, dash ='dash'))
                      ,row = R ,col = C )
        
        fig.add_trace(
                       go.Scatter(x=[Valmax,Valmin], y=[Chainage[maxInd], Chainage[minInd]], mode="markers+text",  text=[round(Valmax,1),round(Valmin,1)],textposition=["bottom right","top right"]),
                       row = R ,col = C
                        )     

        #RMaxy =  max(abs(Valmax), abs(Valmin))
        #RMiny =  -max(abs(Valmax), abs(Valmin))
        
        fig.update_yaxes(title_text='Chainage[m]', range= [RMin,RMax])
        #fig.update_xaxes( range= [RMiny,RMaxy])
       
        
        
        fig.update_layout( template="plotly_white")                
        
    fig.update_layout(height=2000, width=1200,showlegend=False,
                  title_text= Title+": LC -"+LoadCase)     

    return fig
