using System.ComponentModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Sample.Models;
using CommunityToolkit.Maui.Sample.ViewModels.Views;
using CommunityToolkit.Maui.Views;
using Microsoft.Extensions.Logging;

namespace CommunityToolkit.Maui.Sample.Pages.Views;

public partial class MediaElementDrmPage : BasePage<MediaElementDrmViewModel>
{
	// ──────────────────────────────────────────────
	//  Axinom public test vectors — v10
	//  https://github.com/Axinom/public-test-vectors
	//  License tokens from ContentKeys/ directory.
	// ──────────────────────────────────────────────

	// CMAF H.264 Single Key (cbcs)
	const string tokenCmafH264SingleKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICIzMDJmODBkZC00MTFlLTQ4ODYtYmNhNS1iYjFmODAxOGEwMjQiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAicm9LQWcwdDdKaTFpNDNmd3YremZ0UT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfQogICAgICBdCiAgICB9LAogICAgImNvbnRlbnRfa2V5X3VzYWdlX3BvbGljaWVzIjogWwogICAgICB7CiAgICAgICAgIm5hbWUiOiAiUG9saWN5IEEiLAogICAgICAgICJwbGF5cmVhZHkiOiB7CiAgICAgICAgICAibWluX2RldmljZV9zZWN1cml0eV9sZXZlbCI6IDE1MCwKICAgICAgICAgICJwbGF5X2VuYWJsZXJzIjogWwogICAgICAgICAgICAiNzg2NjI3RDgtQzJBNi00NEJFLThGODgtMDhBRTI1NUIwMUE3IgogICAgICAgICAgXQogICAgICAgIH0KICAgICAgfQogICAgXQogIH0KfQ._NfhLVY7S6k8TJDWPeMPhUawhympnrk6WAZHOVjER6M";

	// CMAF H.264 Multi Key (cbcs)
	const string tokenCmafH264MultiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICJiNTRlYzkxNC0xOTJkLTRlYTEtYWMxOS1mNDI5ZWI0OTgyNjgiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAiR1ZERnJZUU9Bb1kzZmpxVVVtamswQT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfSwKICAgICAgICB7CiAgICAgICAgICAiaWQiOiAiYzgzYzRlYTgtMGYyYS00NTIzLTg1MWMtZmJlY2NkYzBmMjAyIiwKICAgICAgICAgICJlbmNyeXB0ZWRfa2V5IjogIlRKZGZsWmJLYmZXQXl5K1dta21UUEE9PSIsCiAgICAgICAgICAidXNhZ2VfcG9saWN5IjogIlBvbGljeSBBIgogICAgICAgIH0sCiAgICAgICAgewogICAgICAgICAgImlkIjogImM4NjhjNzAyLWM3MWItNDA2NC1hZTJiLWMyNGY3Y2MxMDc5MiIsCiAgICAgICAgICAiZW5jcnlwdGVkX2tleSI6ICJ4QXJpUkpOcUFTdXp6RExDRzNXSjdnPT0iLAogICAgICAgICAgInVzYWdlX3BvbGljeSI6ICJQb2xpY3kgQSIKICAgICAgICB9CiAgICAgIF0KICAgIH0sCiAgICAiY29udGVudF9rZXlfdXNhZ2VfcG9saWNpZXMiOiBbCiAgICAgIHsKICAgICAgICAibmFtZSI6ICJQb2xpY3kgQSIsCiAgICAgICAgInBsYXlyZWFkeSI6IHsKICAgICAgICAgICJtaW5fZGV2aWNlX3NlY3VyaXR5X2xldmVsIjogMTUwLAogICAgICAgICAgInBsYXlfZW5hYmxlcnMiOiBbCiAgICAgICAgICAgICI3ODY2MjdEOC1DMkE2LTQ0QkUtOEY4OC0wOEFFMjU1QjAxQTciCiAgICAgICAgICBdCiAgICAgICAgfQogICAgICB9CiAgICBdCiAgfQp9.XC0YIbZpKGFc3IZROklP4LvISc6cZGpE9UL-XcpcqWg";

	// CMAF H.265 Single Key
	const string tokenCmafH265SingleKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICJhYmNjNDRlNS1jMTIyLTQ1YWItYWM4MC1hNWIzNTIyYTBhMzEiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAiZnM2VUx1UzR3SFQxdkI2M0RONnI5UT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfQogICAgICBdCiAgICB9LAogICAgImNvbnRlbnRfa2V5X3VzYWdlX3BvbGljaWVzIjogWwogICAgICB7CiAgICAgICAgIm5hbWUiOiAiUG9saWN5IEEiLAogICAgICAgICJwbGF5cmVhZHkiOiB7CiAgICAgICAgICAibWluX2RldmljZV9zZWN1cml0eV9sZXZlbCI6IDE1MCwKICAgICAgICAgICJwbGF5X2VuYWJsZXJzIjogWwogICAgICAgICAgICAiNzg2NjI3RDgtQzJBNi00NEJFLThGODgtMDhBRTI1NUIwMUE3IgogICAgICAgICAgXQogICAgICAgIH0KICAgICAgfQogICAgXQogIH0KfQ.5rM_qUo4dKrHNDKQO0yzbCiufJxFUzHeOQc13Z48rv4";

	// CMAF H.265 Multi Key
	const string tokenCmafH265MultiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICI1M2RjM2VhYS01MTY0LTQxMGEtOGY0ZS1lMTUxMTNiNDMwNDAiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAiSk00UnNXR0M5dVpjd1llRk5NakNPdz09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfSwKICAgICAgICB7CiAgICAgICAgICAiaWQiOiAiOWRiYWNlOWUtNDEwMy00YzUyLTk2YWEtNjMyMjdkYzVmNzczIiwKICAgICAgICAgICJlbmNyeXB0ZWRfa2V5IjogInliTUNkUkRnamgvR215cG9mTVdDa3c9PSIsCiAgICAgICAgICAidXNhZ2VfcG9saWN5IjogIlBvbGljeSBBIgogICAgICAgIH0sCiAgICAgICAgewogICAgICAgICAgImlkIjogImE3NmYwY2E2LThlN2QtNDBkMC04YTM3LTkwNmYzZTI0ZGRlMiIsCiAgICAgICAgICAiZW5jcnlwdGVkX2tleSI6ICJTTnlTSFlEZ3MzYkJtamhPTlh5SmRBPT0iLAogICAgICAgICAgInVzYWdlX3BvbGljeSI6ICJQb2xpY3kgQSIKICAgICAgICB9CiAgICAgIF0KICAgIH0sCiAgICAiY29udGVudF9rZXlfdXNhZ2VfcG9saWNpZXMiOiBbCiAgICAgIHsKICAgICAgICAibmFtZSI6ICJQb2xpY3kgQSIsCiAgICAgICAgInBsYXlyZWFkeSI6IHsKICAgICAgICAgICJtaW5fZGV2aWNlX3NlY3VyaXR5X2xldmVsIjogMTUwLAogICAgICAgICAgInBsYXlfZW5hYmxlcnMiOiBbCiAgICAgICAgICAgICI3ODY2MjdEOC1DMkE2LTQ0QkUtOEY4OC0wOEFFMjU1QjAxQTciCiAgICAgICAgICBdCiAgICAgICAgfQogICAgICB9CiAgICBdCiAgfQp9.SSRguglJk2l3VahbSq8N5O4Qhxv78n2gSL5Za8HZJmk";

	// DASH / HLS H.264 Single Key
	const string tokenDashH264SingleKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICI0MDYwYTg2NS04ODc4LTQyNjctOWNiZi05MWFlNWJhZTFlNzIiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAid3QzRW51dVI1UkFybjZBRGYxNkNCQT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfQogICAgICBdCiAgICB9LAogICAgImNvbnRlbnRfa2V5X3VzYWdlX3BvbGljaWVzIjogWwogICAgICB7CiAgICAgICAgIm5hbWUiOiAiUG9saWN5IEEiLAogICAgICAgICJwbGF5cmVhZHkiOiB7CiAgICAgICAgICAibWluX2RldmljZV9zZWN1cml0eV9sZXZlbCI6IDE1MCwKICAgICAgICAgICJwbGF5X2VuYWJsZXJzIjogWwogICAgICAgICAgICAiNzg2NjI3RDgtQzJBNi00NEJFLThGODgtMDhBRTI1NUIwMUE3IgogICAgICAgICAgXQogICAgICAgIH0KICAgICAgfQogICAgXQogIH0KfQ.l8PnZznspJ6lnNmfAE9UQV532Ypzt1JXQkvrk8gFSRw";

	// DASH / HLS H.264 Multi Key (cenc)
	const string tokenDashH264MultiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICI0MjZkMWEzMi03OGZkLTRmMjItODczMC02OGRiMzk3NGRkYTkiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAiZjFsLy95M0dnN3pFVE9qM1ZQTXovQT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfSwKICAgICAgICB7CiAgICAgICAgICAiaWQiOiAiOWRjOGU4MGEtY2JmYS00MWMzLTk4NGYtYjYwNDM0NDAzOTFhIiwKICAgICAgICAgICJlbmNyeXB0ZWRfa2V5IjogInlxOW9pSjJ0QnQ1bkpFM1VENE53bXc9PSIsCiAgICAgICAgICAidXNhZ2VfcG9saWN5IjogIlBvbGljeSBBIgogICAgICAgIH0sCiAgICAgICAgewogICAgICAgICAgImlkIjogIjQxYmFhNTk5LTY5MDUtNGZjMC1hOGM2LTM1NWRjZDFhYjM5ZiIsCiAgICAgICAgICAiZW5jcnlwdGVkX2tleSI6ICJ0ZWhGVGhwK2RpMUFHSHM2eGdySjBRPT0iLAogICAgICAgICAgInVzYWdlX3BvbGljeSI6ICJQb2xpY3kgQSIKICAgICAgICB9CiAgICAgIF0KICAgIH0sCiAgICAiY29udGVudF9rZXlfdXNhZ2VfcG9saWNpZXMiOiBbCiAgICAgIHsKICAgICAgICAibmFtZSI6ICJQb2xpY3kgQSIsCiAgICAgICAgInBsYXlyZWFkeSI6IHsKICAgICAgICAgICJtaW5fZGV2aWNlX3NlY3VyaXR5X2xldmVsIjogMTUwLAogICAgICAgICAgInBsYXlfZW5hYmxlcnMiOiBbCiAgICAgICAgICAgICI3ODY2MjdEOC1DMkE2LTQ0QkUtOEY4OC0wOEFFMjU1QjAxQTciCiAgICAgICAgICBdCiAgICAgICAgfQogICAgICB9CiAgICBdCiAgfQp9.KpLCxibrW87lZwA_CSuZdqj7u0L-lnt-e3z_M1Toas0";

	// DASH / HLS H.265 Single Key
	const string tokenDashH265SingleKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICI5ZmQzODVkNS1mMzg5LTQ4YjUtYjdjMy1iMTg2M2VlMTA4ODgiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAiS3ZhaytZZVF1NGU2QnRvcEQ2Wm1JUT09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfQogICAgICBdCiAgICB9LAogICAgImNvbnRlbnRfa2V5X3VzYWdlX3BvbGljaWVzIjogWwogICAgICB7CiAgICAgICAgIm5hbWUiOiAiUG9saWN5IEEiLAogICAgICAgICJwbGF5cmVhZHkiOiB7CiAgICAgICAgICAibWluX2RldmljZV9zZWN1cml0eV9sZXZlbCI6IDE1MCwKICAgICAgICAgICJwbGF5X2VuYWJsZXJzIjogWwogICAgICAgICAgICAiNzg2NjI3RDgtQzJBNi00NEJFLThGODgtMDhBRTI1NUIwMUE3IgogICAgICAgICAgXQogICAgICAgIH0KICAgICAgfQogICAgXQogIH0KfQ.CNEEm6UhOFiXadbcxQrs64NEb9ys7YdPZ7TmTO8aTbg";

	// DASH / HLS H.265 Multi Key
	const string tokenDashH265MultiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJ2ZXJzaW9uIjogMSwKICAiY29tX2tleV9pZCI6ICI2OWU1NDA4OC1lOWUwLTQ1MzAtOGMxYS0xZWI2ZGNkMGQxNGUiLAogICJtZXNzYWdlIjogewogICAgInR5cGUiOiAiZW50aXRsZW1lbnRfbWVzc2FnZSIsCiAgICAidmVyc2lvbiI6IDIsCiAgICAibGljZW5zZSI6IHsKICAgICAgImFsbG93X3BlcnNpc3RlbmNlIjogdHJ1ZQogICAgfSwKICAgICJjb250ZW50X2tleXNfc291cmNlIjogewogICAgICAiaW5saW5lIjogWwogICAgICAgIHsKICAgICAgICAgICJpZCI6ICIzMWJiNjViNC01ODMxLTRjMzQtOTExNC0yNTU5MWJhZTQwNjYiLAogICAgICAgICAgImVuY3J5cHRlZF9rZXkiOiAiWTV2ZDB2aWpvbDExVHFMKytBTFpNZz09IiwKICAgICAgICAgICJ1c2FnZV9wb2xpY3kiOiAiUG9saWN5IEEiCiAgICAgICAgfSwKICAgICAgICB7CiAgICAgICAgICAiaWQiOiAiZDVlM2YzNjctZmM5Ny00Njg1LWFjM2QtMmNjYWI0ZTAxNDhhIiwKICAgICAgICAgICJlbmNyeXB0ZWRfa2V5IjogInhwM1l6TWpQTkFDT2FSeEoxRnJiV3c9PSIsCiAgICAgICAgICAidXNhZ2VfcG9saWN5IjogIlBvbGljeSBBIgogICAgICAgIH0sCiAgICAgICAgewogICAgICAgICAgImlkIjogIjk4MjkzYWE1LWE2M2QtNDA1Ni1hZWI1LWI4ZWJmMmIyMjM3MCIsCiAgICAgICAgICAiZW5jcnlwdGVkX2tleSI6ICJYZmpUZkFqZjYxSk9JK1BuM0hIV0dnPT0iLAogICAgICAgICAgInVzYWdlX3BvbGljeSI6ICJQb2xpY3kgQSIKICAgICAgICB9CiAgICAgIF0KICAgIH0sCiAgICAiY29udGVudF9rZXlfdXNhZ2VfcG9saWNpZXMiOiBbCiAgICAgIHsKICAgICAgICAibmFtZSI6ICJQb2xpY3kgQSIsCiAgICAgICAgInBsYXlyZWFkeSI6IHsKICAgICAgICAgICJtaW5fZGV2aWNlX3NlY3VyaXR5X2xldmVsIjogMTUwLAogICAgICAgICAgInBsYXlfZW5hYmxlcnMiOiBbCiAgICAgICAgICAgICI3ODY2MjdEOC1DMkE2LTQ0QkUtOEY4OC0wOEFFMjU1QjAxQTciCiAgICAgICAgICBdCiAgICAgICAgfQogICAgICB9CiAgICBdCiAgfQp9.8U5sx_tcQOUb86cjqkd5e2leudsnT4BpzY0zTTVAKcA";

	// ──────────────────────────────────────────────
	//  Test vectors catalogue
	// ──────────────────────────────────────────────
	static readonly DrmTestVector[] testVectors =
	[
		new("CMAF H.264 Single Key",  "CMAF",
			"H.264 | cbcs | 1 key | 1080p",
			"https://media.axprod.net/TestVectors/Cmaf/protected_1080p_h264_cbcs/manifest.mpd",
			"https://media.axprod.net/TestVectors/Cmaf/protected_1080p_h264_cbcs/manifest.m3u8",
			tokenCmafH264SingleKey),

		new("CMAF H.264 Multi Key",   "CMAF",
			"H.264 | cbcs | 3 keys (video+audio+text) | 1080p",
			"https://media.axprod.net/TestVectors/MultiKey/Cmaf_h264_1080p_cbcs/manifest.mpd",
			"https://media.axprod.net/TestVectors/MultiKey/Cmaf_h264_1080p_cbcs/manifest.m3u8",
			tokenCmafH264MultiKey),

		new("CMAF H.265 Single Key",  "CMAF",
			"H.265 | cbcs | 1 key | 1080p",
			"https://media.axprod.net/TestVectors/H265/protected_cmaf_1080p_h265_singlekey/manifest.mpd",
			"https://media.axprod.net/TestVectors/H265/protected_cmaf_1080p_h265_singlekey/manifest.m3u8",
			tokenCmafH265SingleKey),

		new("CMAF H.265 Multi Key",   "CMAF",
			"H.265 | cbcs | 3 keys (video+audio+text) | 1080p",
			"https://media.axprod.net/TestVectors/H265/protected_cmaf_1080p_h265_multikey/manifest.mpd",
			"https://media.axprod.net/TestVectors/H265/protected_cmaf_1080p_h265_multikey/manifest.m3u8",
			tokenCmafH265MultiKey),

		new("DASH H.264 Single Key",  "DASH",
			"H.264 | cenc | 1 key | 1080p",
			"https://media.axprod.net/TestVectors/Dash/protected_dash_1080p_h264_singlekey/manifest.mpd",
			"",
			tokenDashH264SingleKey),

		new("DASH H.264 Multi Key",   "DASH",
			"H.264 | cenc | 3 keys (video+audio+text) | 1080p",
			"https://media.axprod.net/TestVectors/MultiKey/Dash_h264_1080p_cenc/manifest.mpd",
			"",
			tokenDashH264MultiKey),

		new("DASH H.265 Single Key",  "DASH",
			"H.265 | cenc | 1 key | 1080p",
			"https://media.axprod.net/TestVectors/H265/protected_dash_1080p_h265_singlekey/manifest.mpd",
			"",
			tokenDashH265SingleKey),

		new("DASH H.265 Multi Key",   "DASH",
			"H.265 | cenc | 3 keys (video+audio+text) | 1080p",
			"https://media.axprod.net/TestVectors/H265/protected_dash_1080p_h265_multikey/manifest.mpd",
			"",
			tokenDashH265MultiKey),

		new("HLS H.264 Single Key",   "HLS",
			"H.264 | 1 key | 1080p | FairPlay only",
			"",
			"https://media.axprod.net/TestVectors/Hls/protected_hls_1080p_h264_singlekey/manifest.m3u8",
			tokenDashH264SingleKey),

		new("HLS H.264 Multi Key",    "HLS",
			"H.264 | cenc | 3 keys | 1080p | FairPlay only",
			"",
			"https://media.axprod.net/TestVectors/MultiKey/Hls_h264_1080p_cenc/manifest.m3u8",
			tokenDashH264MultiKey),

		new("HLS H.265 Single Key",   "HLS",
			"H.265 | 1 key | 1080p | FairPlay only",
			"",
			"https://media.axprod.net/TestVectors/H265/protected_hls_1080p_h265_singlekey/manifest.m3u8",
			tokenDashH265SingleKey),

		new("HLS H.265 Multi Key",    "HLS",
			"H.265 | 3 keys | 1080p | FairPlay only",
			"",
			"https://media.axprod.net/TestVectors/H265/protected_hls_1080p_h265_multikey/manifest.m3u8",
			tokenDashH265MultiKey),

		new("Clear DASH (no DRM)",     "Clear",
			"H.264 | AAC | No encryption | DASH-IF reference",
			"https://dash.akamaized.net/akamai/bbb_30fps/bbb_30fps.mpd",
			"",
			""), // No license token — clear content
	];

	// ──────────────────────────────────────────────
	//  State
	// ──────────────────────────────────────────────

	readonly ILogger logger;
	readonly Dictionary<string, string> drmHeaders = new(StringComparer.OrdinalIgnoreCase);

	DrmTestVector? selectedTestVector;

	public MediaElementDrmPage(MediaElementDrmViewModel viewModel, ILogger<MediaElementDrmPage> logger) : base(viewModel)
	{
		InitializeComponent();

		this.logger = logger;
		MediaElement.PropertyChanged += MediaElement_PropertyChanged;

		InitializeDrmDefaults();
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);
		MediaElement.Stop();
		MediaElement.Handler?.DisconnectHandler();
	}

	void InitializeDrmDefaults()
	{
		// Default to first test vector (CMAF H.264 Single Key)
		TestVectorPicker.SelectedIndex = 0;

		// Default to software DRM (SL2000) — works on dev machines without HDCP.
		// Hardware DRM (SL3000) requires TEE + HDCP-compliant GPU/monitor.
		DrmSecurityLevelPicker.SelectedIndex = 0; // Software DRM

		// Auto-select the appropriate DRM scheme for the current platform
		if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			DrmSchemePicker.SelectedIndex = 1; // PlayReady
			PlatformHintLabel.Text = "(recommended for Windows)";
		}
		else if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			DrmSchemePicker.SelectedIndex = 0; // Widevine
			PlatformHintLabel.Text = "(recommended for Android)";
		}
		else if (DeviceInfo.Platform == DevicePlatform.iOS
				 || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
		{
			DrmSchemePicker.SelectedIndex = 2; // FairPlay
			PlatformHintLabel.Text = "(required for iOS/macOS)";
		}

		ApplySelections();
	}

	// ──────────────────────────────────────────────
	//  Test vector selection
	// ──────────────────────────────────────────────

	void OnTestVectorChanged(object? sender, EventArgs e)
	{
		if (TestVectorPicker.SelectedIndex < 0)
		{
			return;
		}

		selectedTestVector = testVectors[TestVectorPicker.SelectedIndex] ?? throw new InvalidOperationException("Selected test vector is null.");

		var scheme = GetSelectedScheme();

		// Auto-switch DRM scheme for format-specific vectors
		if (selectedTestVector.Category == "HLS")
		{
			// HLS-only vectors need FairPlay
			DrmSchemePicker.SelectedIndex = 2; // FairPlay
			PlatformHintLabel.Text = "(HLS → FairPlay required)";
		}
		// Warn if CMAF (cbcs) is selected with PlayReady — the PlayReady engine
		// expects traditional DASH SegmentTemplate, not CMAF cbcs layout.
		else if (scheme is DrmScheme.PlayReady && selectedTestVector.Category == "CMAF")
		{
			PlatformHintLabel.Text = "⚠ CMAF cbcs unsupported with PlayReady — use DASH instead";
		}

		ApplySelections();
	}

	// ──────────────────────────────────────────────
	//  DRM scheme selection
	// ──────────────────────────────────────────────

	void OnDrmSchemeChanged(object? sender, EventArgs e)
	{
		var scheme = GetSelectedScheme();
		var tvIndex = TestVectorPicker.SelectedIndex;

		PlatformHintLabel.Text = scheme switch
		{
			DrmScheme.Widevine => "(Google Widevine — Android, Chrome)",
			DrmScheme.PlayReady => "(Microsoft PlayReady — Windows, Xbox)",
			DrmScheme.FairPlay => "(Apple FairPlay — iOS, macOS, Safari)",
			_ => "",
		};

		// PlayReady on Windows does not support CMAF cbcs segment layouts
		// (it expects traditional SegmentTemplate from DASH manifests).
		// Auto-switch CMAF → DASH when PlayReady is selected.
		if (scheme is DrmScheme.PlayReady
			&& tvIndex >= 0 && tvIndex < 4) // CMAF indices: 0-3
		{
			TestVectorPicker.SelectedIndex = tvIndex + 4; // switch to DASH counterpart
			PlatformHintLabel.Text += " — DASH preferred (CMAF cbcs unsupported)";
		}

		// For HLS-only vectors on non-Apple platforms, guide the user
		if (scheme is not DrmScheme.FairPlay
			&& tvIndex >= 0
			&& testVectors[tvIndex].Category == "HLS"
			&& string.IsNullOrWhiteSpace(testVectors[tvIndex].DashManifestUrl))
		{
			PlatformHintLabel.Text += " ⚠ HLS-only vector — switch to FairPlay";
		}

		ApplySelections();
	}

	// ──────────────────────────────────────────────
	//  Apply current selections to the UI
	// ──────────────────────────────────────────────

	void ApplySelections()
	{
		if (TestVectorPicker.SelectedIndex < 0)
		{
			return;
		}

		selectedTestVector = testVectors[TestVectorPicker.SelectedIndex];
		var scheme = GetSelectedScheme();

		// ── License server URL ──────────────────────────
		// Each DRM scheme has its own license service endpoint.
		// Axinom provides separate URLs for Widevine, PlayReady, and FairPlay.
		//
		// PlayReady (Windows via Media Foundation):
		//   The MediaElement internally creates a PlayReady CDM via
		//   Windows.Media.Protection.MediaProtectionManager. The CDM generates
		//   a PlayReady challenge XML containing a LicenseAcquisition/challenge.
		//   MediaElement POSTs the challenge to this LicenseServerUrl with the
		//   LicenseRequestHeaders (e.g. X-AxDRM-Message) attached, then feeds
		//   the license response back to the CDM to complete key acquisition.
		//   See: https://learn.microsoft.com/en-us/windows/win32/medfound/how-to-play-protected-media-files
		//
		// Widevine (Android via ExoPlayer):
		//   ExoPlayer sends a Widevine license request to the LicenseServerUrl.
		//   LicenseRequestHeaders are included in the HTTP POST.
		//
		// FairPlay (iOS/macOS via AVAssetResourceLoaderDelegate):
		//   The app receives an SPC (Server Playback Context) from the player,
		//   sends it to the FairPlay license server, and returns the CKC
		//   (Content Key Context) to the player.
		LicenseServerEntry.Text = selectedTestVector.Category == "Clear"
			? ""
			: scheme switch
			{
				DrmScheme.Widevine => "https://drm-widevine-licensing.axprod.net/AcquireLicense",
				DrmScheme.PlayReady => "https://drm-playready-licensing.axprod.net/AcquireLicense",
				DrmScheme.FairPlay => "https://drm-fairplay-licensing.axprod.net/AcquireLicense",
				_ => "",
			};

		// ── Source URL ───────────────────────────────────
		// Use MPD for Widevine/PlayReady (DASH/CMAF), M3U8 for FairPlay (HLS/CMAF).
		SourceUrlEntry.Text = scheme switch
		{
			DrmScheme.FairPlay => string.IsNullOrWhiteSpace(selectedTestVector.HlsManifestUrl)
				? selectedTestVector.DashManifestUrl   // fallback
				: selectedTestVector.HlsManifestUrl,
			_ => string.IsNullOrWhiteSpace(selectedTestVector.DashManifestUrl)
				? selectedTestVector.HlsManifestUrl    // fallback (HLS-only vectors)
				: selectedTestVector.DashManifestUrl,
		};

		// ── License request headers ──────────────────────
		// Axinom requires the X-AxDRM-Message JWT token on every license
		// request for Widevine and PlayReady. The token embeds the content
		// key(s) and usage policy. For FairPlay, the token is sent as part
		// of the SPC exchange.
		drmHeaders.Clear();
		if (!string.IsNullOrWhiteSpace(selectedTestVector.LicenseToken))
		{
			drmHeaders["X-AxDRM-Message"] = selectedTestVector.LicenseToken;
		}

		UpdateDrmHeadersSummary();
	}

	// ──────────────────────────────────────────────
	//  Helpers
	// ──────────────────────────────────────────────

	DrmScheme GetSelectedScheme() => DrmSchemePicker.SelectedIndex switch
	{
		0 => DrmScheme.Widevine,
		1 => DrmScheme.PlayReady,
		2 => DrmScheme.FairPlay,
		_ => DrmScheme.Unknown,
	};

	void MediaElement_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == MediaElement.DurationProperty.PropertyName)
		{
			logger.LogInformation("DRM Media — Duration: {NewDuration}", MediaElement.Duration);
			PositionSlider.Maximum = MediaElement.Duration.TotalSeconds;
		}
	}

	void OnMediaOpened(object? sender, EventArgs? e)
	{
		logger.LogInformation("DRM Media opened.");
		DimensionsText.Text = $"{MediaElement.MediaWidth}x{MediaElement.MediaHeight}";
	}

	void OnStateChanged(object? sender, MediaStateChangedEventArgs e)
	{
		logger.LogInformation("DRM Media State Changed: {Previous} → {New}", e.PreviousState, e.NewState);
	}

	void OnMediaFailed(object? sender, MediaFailedEventArgs e)
	{
		logger.LogError("DRM Media failed: {ErrorMessage}", e.ErrorMessage);
	}

	void OnMediaEnded(object? sender, EventArgs? e) => logger.LogInformation("DRM Media ended.");

	void OnPositionChanged(object? sender, MediaPositionChangedEventArgs e)
	{
		PositionSlider.Value = e.Position.TotalSeconds;
	}

	void OnSeekCompleted(object? sender, EventArgs? e) => logger.LogInformation("Seek completed.");

	void OnPlayClicked(object? sender, EventArgs? e) => MediaElement.Play();

	void OnPauseClicked(object? sender, EventArgs? e) => MediaElement.Pause();

	void OnStopClicked(object? sender, EventArgs? e) => MediaElement.Stop();

	void OnMuteClicked(object? sender, EventArgs? e) => MediaElement.ShouldMute = !MediaElement.ShouldMute;

	async void Slider_DragCompleted(object? sender, EventArgs? e)
	{
		ArgumentNullException.ThrowIfNull(sender);

		var newValue = ((Slider)sender).Value;
		await MediaElement.SeekTo(TimeSpan.FromSeconds(newValue), CancellationToken.None);

		MediaElement.Play();
	}

	void Slider_DragStarted(object? sender, EventArgs? e)
	{
		MediaElement.Pause();
	}

	/// <summary>
	/// Builds and loads the DRM-protected source from the current UI fields.
	///
	/// <para><b>PlayReady on Windows (Media Foundation EME/CDM flow):</b></para>
	/// <para>
	/// Based on the PlayReady EME approach described at
	/// https://stackoverflow.com/a/76769572 — the MediaElement internally:
	/// </para>
	/// <list type="number">
	/// <item><description>
	///   Creates an <c>IMFContentDecryptionModule</c> with key system
	///   <c>"com.microsoft.playready"</c> and configures it with
	///   <c>MF_EME_INITDATATYPES = ["cenc"]</c> (or <c>"cbcs"</c> for CMAF).
	///   For non-UWP (Win32) apps, it also sets up an <c>IMFPMPHostApp</c>
	///   so the CDM can activate objects inside the Protected Media Path (PMP).
	/// </description></item>
	/// <item><description>
	///   Creates a <c>Windows.Media.Protection.MediaProtectionManager</c> and
	///   wires it into the Media Foundation pipeline via
	///   <c>IMFMediaEngineProtectedContent::SetContentProtectionManager</c>.
	/// </description></item>
	/// <item><description>
	///   When the CDM fires <c>KeyMessage</c>, the MediaElement parses the
	///   PlayReady challenge XML, extracting:
	///   <c>PlayReadyKeyMessage / LicenseAcquisition / Challenge</c> (base64)
	///   and <c>… / HttpHeaders</c>. It then POSTs the decoded challenge to
	///   <see cref="DrmConfiguration.LicenseServerUrl"/> with the
	///   <see cref="DrmConfiguration.LicenseRequestHeaders"/> (here:
	///   <c>X-AxDRM-Message</c> JWT token) appended.
	/// </description></item>
	/// <item><description>
	///   The license server response is fed back via
	///   <c>IMFContentDecryptionModuleSession::Update</c>. The CDM then
	///   reports key status via <c>KeyStatusChanged</c> (each key should
	///   reach <c>MF_MEDIAKEY_STATUS_USABLE</c> before playback starts).
	/// </description></item>
	/// </list>
	///
	/// <para><b>Widevine on Android (ExoPlayer):</b></para>
	/// <para>
	/// ExoPlayer sends a Widevine license request with the PSSH box to the
	/// license server. <c>LicenseRequestHeaders</c> are included as HTTP headers.
	/// </para>
	///
	/// <para><b>FairPlay on iOS/macOS (AVFoundation):</b></para>
	/// <para>
	/// AVAssetResourceLoaderDelegate receives an SPC (Server Playback Context),
	/// sends it to the FairPlay license server with headers, and loads the
	/// returned CKC (Content Key Context) for decryption.
	/// </para>
	/// </summary>
	void LoadDrmSource()
	{
		var sourceUrl = SourceUrlEntry.Text?.Trim();
		if (string.IsNullOrWhiteSpace(sourceUrl))
		{
			logger.LogWarning("No source URL provided for DRM load.");
			return;
		}

		if (!Uri.TryCreate(sourceUrl, UriKind.Absolute, out var sourceUri))
		{
			logger.LogWarning("Invalid source URL: {Url}", sourceUrl);
			return;
		}

		var scheme = GetSelectedScheme();
		if (scheme is DrmScheme.Unknown)
		{
			logger.LogWarning("No DRM scheme selected.");
			return;
		}

		// ── DRM Configuration ─────────────────────────
		//
		// DrmConfiguration is the platform-agnostic DRM facade.
		// At runtime the MediaElement translates it into:
		//   Windows:  IMFContentDecryptionModule + MediaProtectionManager
		//             (key system: "com.microsoft.playready")
		//   Android:  ExoPlayer MediaItem.DrmConfiguration
		//             (key system: "com.widevine.alpha" or "com.microsoft.playready")
		//   iOS/macOS: AVAssetResourceLoaderDelegate (FairPlay Streaming)
		var drmConfig = new DrmConfiguration
		{
			Scheme = scheme,
			RequiresHardwareSecurity = DrmSecurityLevelPicker.SelectedIndex == 1,
		};

		var licenseUrl = LicenseServerEntry.Text?.Trim();
		if (!string.IsNullOrWhiteSpace(licenseUrl) && Uri.TryCreate(licenseUrl, UriKind.Absolute, out var licenseUri))
		{
			drmConfig.LicenseServerUrl = licenseUri;
		}

		// License request headers — these are sent with every license
		// acquisition HTTP POST, regardless of DRM scheme.
		// For Axinom: X-AxDRM-Message is a JWT embedding the content key
		// IDs and usage policy (Policy A: min SL150, persistence allowed).
		foreach (var header in drmHeaders)
		{
			drmConfig.LicenseRequestHeaders[header.Key] = header.Value;
		}

		var uriMediaSource = new UriMediaSource
		{
			Uri = sourceUri,
			DrmConfiguration = drmConfig,
		};

		if (selectedTestVector is null)
		{
			throw new InvalidOperationException("Selected test vector is null.");
		}

		logger.LogInformation(
			"Loading DRM source — Vector: {Vector}, Scheme: {Scheme}, URL: {Url}, License: {LicenseUrl}, HW: {Hw}",
			selectedTestVector.Name, scheme, sourceUri, drmConfig.LicenseServerUrl, drmConfig.RequiresHardwareSecurity);

		MediaElement.Source = uriMediaSource;
	}

	void OnLoadDrmSourceClicked(object? sender, EventArgs? e) => LoadDrmSource();

	void ClearDrmHeadersClicked(object? sender, EventArgs? e)
	{
		drmHeaders.Clear();
		UpdateDrmHeadersSummary();
	}

	void UpdateDrmHeadersSummary()
	{
		DrmHeadersSummaryLabel.Text = drmHeaders.Count <= 0
			? "No headers defined"
			: string.Join(", ", drmHeaders.Keys);
	}
}
