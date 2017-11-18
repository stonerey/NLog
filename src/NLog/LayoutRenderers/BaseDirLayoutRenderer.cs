// 
// Copyright (c) 2004-2017 Jaroslaw Kowalski <jaak@jkowalski.net>, Kim Christensen, Julian Verdurmen
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

namespace NLog.LayoutRenderers
{
    using System;
    using System.IO;
    using System.Text;
    using Internal.Fakeables;
    using NLog.Config;
    using NLog.Internal;

    /// <summary>
    /// The current application domain's base directory.
    /// </summary>
    [LayoutRenderer("basedir")]
    [AppDomainFixedOutput]
    [ThreadAgnostic]
    public class BaseDirLayoutRenderer : LayoutRenderer
    {
        private readonly string _baseDir;

#if !SILVERLIGHT

        /// <summary>
        /// cached
        /// </summary>
        private string _processDir;

        /// <summary>
        /// Use base dir of current process.
        /// </summary>
        public bool ProcessDir { get; set; }

#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDirLayoutRenderer" /> class.
        /// </summary>
        public BaseDirLayoutRenderer() : this(LogFactory.CurrentAppDomain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDirLayoutRenderer" /> class.
        /// </summary>
        public BaseDirLayoutRenderer(IAppDomain appDomain)
        {
            _baseDir = appDomain.BaseDirectory;
        }

        /// <summary>
        /// Gets or sets the name of the file to be Path.Combine()'d with with the base directory.
        /// </summary>
        /// <docgen category='Advanced Options' order='10' />
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the name of the directory to be Path.Combine()'d with with the base directory.
        /// </summary>
        /// <docgen category='Advanced Options' order='10' />
        public string Dir { get; set; }

        /// <summary>
        /// Renders the application base directory and appends it to the specified <see cref="StringBuilder" />.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to append the rendered data to.</param>
        /// <param name="logEvent">Logging event.</param>
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {

            var dir = _baseDir;
#if !SILVERLIGHT
            if (ProcessDir)
            {
                dir = _processDir ?? (_processDir = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
            }
#endif

            if (dir != null)
            {
                var path = PathHelpers.CombinePaths(dir, Dir, File);
                builder.Append(path);
            }
        }
    }
}

