#include <iostream>
#include<math.h>

using namespace std;

int main()
{
    float Returns[10][5]={{2.53, 56.91, -1.61, 14.33, 19.68}, 
        {-0.14, 29.63, 6.29, -18.63, 1.93}, 
        {17.58, -0.18, -7.30, 23.22, 13.41},
        {1.92, 9.44, 32.10, 16.26, 18.45},
        {9.54, 49.36, 6.55, -1.79, 10.35},
        {-3.13, -18.55, 31.01, 9.00, -4.59},
        {6.98, 22.08, 16.00, 35.23, 17.62},
        {-1.45, 32.19, 38.50, 31.25, 20.66},
        {11.09,-0.60, 9.45, -11.12,-4.11},
        {25.60, 62.04, 48.86, 24.60, 16.62}};

    float Std[5]={9.14, 26.85,18.59, 17.92, 9.77};
    float Wt[5]={0.15,0.15,0.15,0.15,0.4};
    float WtStd[5]={0};
    float M1[5]={0};
    float M2, Risk, Return, RbyR;
    float AvgR[5]={0};
    float CoVar[5][5]={{0}};
    float CoRel[5][5]={{0}};

    int i, j,k;
    for(i=0; i<=4; i++)
    {
        for(j=0; j<=9; j++)
        {
            AvgR[i]=AvgR[i]+Returns[j][i];
        }
        AvgR[i]=AvgR[i]/10;
    }
    for(i=0; i<=10; i++)
    {
        for(j=0; j<=4; j++)
        {
            Returns[i][j]=Returns[i][j]-AvgR[j];
        }

    }
    for(i=0; i<=4; i++)
    {
        for(j=0; j<=4; j++)
        {
            for(k=0; k<=9; k++)
            { 
                CoVar[i][j]=CoVar[i][j]+Returns[k][j]*Returns[k][i];
            }
            CoVar[i][j]=CoVar[i][j]/9;
        }
    }
    for(i=0; i<=4; i++)
    {
        for(j=0; j<=4; j++)
        {
            CoRel[i][j]=CoVar[i][j]/(Std[i]*Std[j]);
        }
    }
    for(i=0; i<=4; i++)
    {
        WtStd[i]=Wt[i]*Std[i];
    }
    for(i=0; i<=4; i++)
    {
        for(j=0; j<=4; j++)
        {
            M1[i]=M1[i]+WtStd[j]*CoRel[j][i];
        }
    }
    for(i=0; i<=4; i++)
    {
        M2=M2+M1[i]*WtStd[i];
        Return=Return+AvgR[i]*Wt[i];
    }
    Risk=sqrt(M2);
    RbyR=Return/Risk;
    cout<<RbyR;
    cout<<"\n";
    return 0;
}


