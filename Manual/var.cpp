#include <iostream>
#include <cmath>
#include <vector>

using namespace std;

int main()
{
    int amt; //Amount to be invested
    float amt_lwr, amt_upr ;// Upper and Lower limit of amt
    float a, b, c, d, e ;//Price of Stocks 
    float a_min, b_min, c_min, d_min, e_min; //Minimum weightage of Stocks
    float a_max, b_max, c_max, d_max, e_max;//Maximum weightage of Stocks
    int u_min, u_max, v_min, v_max, x_min, x_max, y_min, y_max, z_min, z_max;
    float i,j,k,l,m;
    int n=0, p;
    cin >> p;
    cin >> amt; 
    cin >> a >> a_min >> a_max;
    cin >> b >> b_min >> b_max;
    cin >> c >> c_min >> c_max;
    cin >> d >> d_min >> d_max;
    cin >> e >> e_min >> e_max;
    amt_lwr=amt*0.99;
    amt_upr=amt*1.01;
    u_min=ceil(a_min*amt)/(a*100);                      
    u_max=floor(a_max*amt)/(a*100);
    v_min=ceil(b_min*amt)/(b*100);
    v_max=floor(b_max*amt)/(b*100);
    x_min=ceil(c_min*amt)/(c*100);
    x_max=floor(c_max*amt)/(c*100);
    y_min=ceil(d_min*amt)/(d*100);
    y_max=floor(d_max*amt)/(d*100);
    z_min=ceil(e_min*amt)/(e*100);
    z_max=floor(e_max*amt)/(e*100);
    for (i=u_min; i<=u_max; i=i+0.5)
    {
        for(j=v_min; j<=v_max; j=j+0.5)
        {
            for(k=x_min; k<=x_max; k=k+0.5)
            {
                for(l=y_min; l<=y_max; l=l+0.5)
                {
                    int portfolioUsed;                                      // Total portfolio consumed by the first 4 stocks
                    int minUnits, maxUnits;
                    portfolioUsed = i*a + j*b + k*c + l*d;
                    minUnits = (amt_lwr - portfolioUsed)/e;                 // minimum units of stock e to be used
                    maxUnits = (amt_upr - portfolioUsed)/e;                 // maximum units of stock e to be used
                    n += (maxUnits - minUnits + 1);
                }
            }
        }
    }
    cout<<"Total number of possible solutions: "<<(long long int)(2*(u_max-u_min)-1)*(2*(v_max-v_min)-1)*(2*(x_max-x_min)-1)*(2*(y_max-y_min)-1)*(2*(z_max-z_min)-1);
    cout<<"\nNumber of valid combinations: "<< n ;
    cout << endl;
    return 0;
}
